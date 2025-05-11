@echo off
setlocal enabledelayedexpansion

:: 获取时间
for /f "tokens=1-4 delims=/ " %%a in ('date /t') do (
    set y=%%a
    set m=%%b
    set d=%%c
)
for /f "tokens=1-2 delims=: " %%e in ('time /t') do (
    set hh=%%e
    set mm=%%f
)

:: 格式化变量
set commitMsg=Auto backup %y%-%m%-%d% %hh%:%mm%
set tagName=tag-%y%-%m%-%d%_%hh%-%mm%
set zipName=backup_%y%-%m%-%d%_%hh%-%mm%.zip

echo.
echo ==== 提交信息: %commitMsg%

:: 添加 + 提交
git add .
git commit -m "%commitMsg%"

:: 处理 tag（防止重复）
git tag -d %tagName% >nul 2>nul
git tag %tagName%

:: 推送 commit 和 tag
git push
git push origin %tagName%

:: 压缩整个目录
powershell Compress-Archive -Path * -DestinationPath %zipName%

:: 发布 Release 并上传压缩包
gh release create %tagName% %zipName% --title "%tagName%" --notes "%commitMsg%"

:: 写入日志
echo [%y%-%m%-%d% %hh%:%mm%] 推送+Release成功: %commitMsg% >> git_push_log.txt

echo.
echo ==== 完成！GitHub 已发布 Release ====
pause
