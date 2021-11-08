using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using TtWork.ProjectName.Apis.Poster;
using AutoMapper;
using MediatR;
using Serilog;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using TTWork.Abp.Core.Oss;
using TTWork.Abp.Oss.UpYun;
using TtWork.Lib;
using TtWork.Lib.Extensions;
using TtWork.ProjectName.Entities.Posters;

namespace TtWork.ProjectName.Events.Queries
{
    public class GetPosterPreviewImageUrl : IRequest<string>
    {
        public int PosterId { get; }


        public string Title { get; set; } = "这个是海报的标题";
        public string Desc { get; set; } = "这个是海报的内容";

        public string QrImgUrl { get; set; } = "Source/qr.jpg";

        public string MainImgUrl { get; set; } = "Source/main.jpg";
        public string HeadimgUrl { get; set; } = "Source/head.jpg";


        public GetPosterPreviewImageUrl(int posterId)
        {
            PosterId = posterId;
        }

        public class GetPosterPreviewImageUrlHandler : IRequestHandler<GetPosterPreviewImageUrl, string>, ITransientDependency
        {
            private static readonly FontFamily fontfamily = new FontCollection().Install("Source/fzlthk.ttf"); //字体的路径

            private readonly IRepository<Poster> Repository;
            private readonly IUnitOfWorkManager _unitOfWorkManager;
            private readonly IUpyunApi _upyunApi;
            private readonly IOssClient _ossClient;
            private readonly IMapper _mapper;
            private readonly HttpClient _qlogoClient;
            private readonly HttpClient _imgClient;

            public GetPosterPreviewImageUrlHandler(
                IRepository<Poster> posterRepository,
                IUnitOfWorkManager unitOfWorkManager,
                IUpyunApi upyunApi,
                IOssClient ossClient,
                IHttpClientFactory httpClientFactory,
                IMapper mapper)
            {
                Repository = posterRepository;
                _unitOfWorkManager = unitOfWorkManager;
                _upyunApi = upyunApi;
                _ossClient = ossClient;
                _mapper = mapper;
                _qlogoClient = httpClientFactory.CreateClient("qlogoClient");
                _imgClient = httpClientFactory.CreateClient("imgClient");
            }

            private IImageDecoder GetDecoder(string url)
            {
                IImageDecoder decoder;
                if (url.IndexOf(".png", StringComparison.Ordinal) > 0)
                    decoder = new PngDecoder();
                else if (url.IndexOf(".gif", StringComparison.Ordinal) > 0)
                    decoder = new GifDecoder();
                else if (url.IndexOf(".bmp", StringComparison.Ordinal) > 0)
                    decoder = new BmpDecoder();
                else
                    decoder = new JpegDecoder();
                return decoder;
            }

            private bool IsHttp(string url)
            {
                return url.StartsWith("http");
            }


            [UnitOfWork]
            public virtual async Task<string> Handle(GetPosterPreviewImageUrl request,
                CancellationToken cancellationToken)
            {
                // var sw1 = new Stopwatch();
                // sw1.Start();
                var find = await Repository.FirstOrDefaultAsync(z => z.Id == request.PosterId);
                if (find == null)
                    throw new UserFriendlyException("NotFind");

                // sw1.Stop();
                // Console.WriteLine("Step1: " + sw1.ElapsedMilliseconds);
                // sw1.Restart();

                var dto = _mapper.Map<PosterDto>(find);
                // sw1.Stop();
                // Console.WriteLine("Step2: " + sw1.ElapsedMilliseconds);
                // sw1.Restart();

                var bytes = await _upyunApi.GetBytesAsync(find.BgImageUrl);
                // sw1.Stop();
                // Console.WriteLine("Step3 httpload: " + sw1.ElapsedMilliseconds);
                // sw1.Restart();

                using var img = Image.Load(bytes, GetDecoder(find.BgImageUrl));
                // sw1.Stop();
                // Console.WriteLine("Step4 Image.Load: " + sw1.ElapsedMilliseconds);
                // sw1.Restart();

                img.Mutate(x =>
                    {
                        x.Resize(dto.BgWidth, dto.BgHeight);
                        x.DrawText(
                            dto.TitleText.GetTextOptions(),
                            request.Title
                            , //文字内容
                            new Font(fontfamily, dto.TitleText.FontSize),
                            dto.TitleText.GetColor(), //文字颜色
                            new PointF(dto.TitleText.X, dto.TitleText.Y)); //坐标位置（浮点）
                        x.DrawText(
                            dto.DescText.GetTextOptions(),
                            request.Desc, //文字内容
                            new Font(fontfamily, dto.DescText.FontSize),
                            dto.DescText.GetColor(), //文字颜色
                            new PointF(dto.DescText.X, dto.DescText.Y)); //坐标位置（浮点）
                    }
                );
                // sw1.Stop();
                // Console.WriteLine("Step5: " + sw1.ElapsedMilliseconds);
                // sw1.Restart();

                #region 合并图片

                using var qrImg = IsHttp(request.QrImgUrl)
                    ? Image.Load(await _imgClient.GetByteArrayAsync(request.QrImgUrl), GetDecoder(request.QrImgUrl))
                    : Image.Load(request.QrImgUrl);
                qrImg.Mutate(x => x.Resize(dto.QrImage.Width, dto.QrImage.Height));
                img.Mutate(a => a.DrawImage(qrImg, new Point(dto.QrImage.X, dto.QrImage.Y), 1f));

                // sw1.Stop();
                // Console.WriteLine("Step6: " + sw1.ElapsedMilliseconds);
                // sw1.Restart();

                try
                {
                    using var mainImg = IsHttp(request.MainImgUrl)
                        ? Image.Load(await _imgClient.GetByteArrayAsync(request.MainImgUrl + "!w500"), GetDecoder(request.MainImgUrl))
                        : Image.Load(request.MainImgUrl);
                    mainImg.Mutate(x => x.Resize(dto.MainImage.Width, dto.MainImage.Height));
                    img.Mutate(a => { a.DrawImage(mainImg, new Point(dto.MainImage.X, dto.MainImage.Y), 1f); });
                }
                catch (Exception e)
                {
                    // this is may be throw exception :SixLabors.ImageSharp.InvalidImageContentException: Missing SOI marker.
                    Log.Error(e.InnerException?.Message ?? e.Message);
                }

                // sw1.Stop();
                // Console.WriteLine("Step7: " + sw1.ElapsedMilliseconds);
                // sw1.Restart();

                using var headerImg =
                    IsHttp(request.HeadimgUrl) ? Image.Load(await _qlogoClient.GetByteArrayAsync(request.HeadimgUrl), GetDecoder(request.HeadimgUrl)) : Image.Load(request.HeadimgUrl);

                headerImg.Mutate(x =>
                    {
                        x.ConvertToAvatar(new Size(dto.HeadImage.Width, dto.HeadImage.Height),
                            dto.HeadImage.Width / 2.0f);
                        x.Resize(dto.HeadImage.Width, dto.HeadImage.Height);
                    }
                );

                img.Mutate(a => { a.DrawImage(headerImg, new Point(dto.HeadImage.X, dto.HeadImage.Y), 1f); });

                // sw1.Stop();
                // Console.WriteLine("Step8: " + sw1.ElapsedMilliseconds);
                // sw1.Restart();
                //
                // sw1.Stop();
                // Console.WriteLine("Step9: " + sw1.ElapsedMilliseconds);
                // sw1.Restart();

                #endregion

                #region 又拍云上传

                var buffer = img.ToArray(PngFormat.Instance);
                var url = $"/{_ossClient.BucketName}/{DateTime.Now:yyyy/MM/HHmmss_}{StringEx.BuildRandomStr(6)}.png";
                //Do whatever you want with filename and its binary data.
                var uploadResult = await _ossClient.writeFile(url, buffer, true);

                if (uploadResult != true)
                    throw new UserFriendlyException("上传文件失败");

                // sw1.Stop();
                // Console.WriteLine("Step10: " + sw1.ElapsedMilliseconds);
                // sw1.Restart();

                #endregion

                await _unitOfWorkManager.Current.SaveChangesAsync();

                // sw1.Stop();
                // Console.WriteLine("Step11: " + sw1.ElapsedMilliseconds);
                // sw1.Restart();

                return $"{_ossClient.Domain}{url}";
            }
        }
    }
}