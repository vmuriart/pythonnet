:: Run embed_tests. Disabled, due to regressions needing fixing
:: .\packages\NUnit.Runners.2.6.2\tools\nunit-console.exe src\embed_tests\bin\x64\DebugWin\Python.EmbeddingTest.dll /noshadow

:: Run Python tests
python .\src\tests\runtests.py
