call npm run build
call lftp ftp://wujiangapp:asdfgg67@106.14.137.103 -e "set ftp:passive-mode off;set ftp:ssl-allow false; mirror -eRn dist ./wjzgh/wwwroot/admin; exit;"