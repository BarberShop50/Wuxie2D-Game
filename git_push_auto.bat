@echo off
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

:: 格式化提交信息与tag名
set commitMsg=Auto backup %y%-%m%-%d% %hh%:%mm%
set tagName=tag-%y%-%m%-%d%_%hh%-%mm%

echo.
echo ==== 提交信息: %commitMsg%
git add .
git commit -m "%commitMsg%"

:: 打标签（防止同名报错先删除旧的）
git tag -d %tagName% >nul 2>nul
git tag %tagName%

echo.
echo ==== 推送到 GitHub...
git push
git push origin %tagName%

:: 写入日志
echo [%y%-%m%-%d% %hh%:%mm%] 推送成功: %commitMsg% >> git_push_log.txt

:: 发布 GitHub Release
gh release create %tagName% --title "%tagName%" --notes "%commitMsg%"

echo.
echo ==== 完成！Release 发布成功 + 日志已更新 ====
pause