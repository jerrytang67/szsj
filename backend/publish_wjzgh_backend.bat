call dotnet publish -c Release ./src/TtWork.ProjectName.Web.Host/TtWork.ProjectName.Web.Host.csproj --output ./publish/web_host
call lftp ftp://wujiangapp:asdfgg67@106.14.137.103 -e "set ftp:ssl-allow false; put -O szsj/ ./app_offline.htm; exit;"
call TIMEOUT /T 5
call lftp ftp://wujiangapp:asdfgg67@106.14.137.103 -e "set ftp:ssl-allow false; mirror -eRn --exclude wwwroot/admin/ --exclude App_Data/ publish/web_host ./szsj; exit;"
