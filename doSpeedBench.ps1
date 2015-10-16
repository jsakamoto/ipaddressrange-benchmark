$score = @()
for ($i=0; $i -le 10; $i++){
    $score += .\benchMarkApp\bin\Debug\benchMarkApp.exe
}
echo $score