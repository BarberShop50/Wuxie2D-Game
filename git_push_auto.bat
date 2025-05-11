@echo off
:: 获取当前时间并格式化
for /f "tokens=1-4 delims=/ " %%a in ('date /t') do (
    set y=%%a
    set m=%%b
    set d=%%c
)
for /f "tokens=1-2 delims=: " %%e in ('time /t') do (
    set hh=%%e
    set mm=%%f
)
:: 拼接成提交信息
set msg=Auto backup %y%-%m%-%d% %hh%:%mm%

echo === 自动提交说明: %msg%
git add .
git commit -m "%msg%"
git push

echo.
echo === 推送完成 ===
pause
