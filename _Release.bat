C:\Factory\Tools\RDMD.exe /RC out

SET RAWKEY=2f1e36c24b9ae5596387bdbef0070ab9

C:\Factory\SubTools\makeAESCluster.exe Picture.txt     out\Data1.dat %RAWKEY% 110000000
C:\Factory\SubTools\makeAESCluster.exe Music.txt       out\Data2.dat %RAWKEY% 120000000
C:\Factory\SubTools\makeAESCluster.exe SoundEffect.txt out\Data3.dat %RAWKEY% 130000000
C:\Factory\SubTools\makeAESCluster.exe Etcetera.txt    out\Data4.dat %RAWKEY% 140000000

COPY /B KiraraScreen\Release\KiraraScreen.exe out\KiraraScreen.exe

out\KiraraScreen.exe /L
IF ERRORLEVEL 1 START ?_LOG_ENABLED

C:\Factory\Tools\xcp.exe doc out

COPY /B Kirara\Kirara\bin\Release\Kirara.exe out
COPY /B C:\Factory\Program\WavMaster\Master.exe out

C:\Factory\SubTools\EmbedConfig.exe --factory-dir-disabled out\Master.exe

C:\Factory\SubTools\zip.exe /O out Kirara

IF NOT "%1" == "/-P" PAUSE
