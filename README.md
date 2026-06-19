# Conference Scheduler

Sistema de agendamento de palestras em C# para gerenciamento de conferências.

O aplicativo organiza as palestras em trilhas, respeitando as regras de tempo:
- Manhã: 09:00 – 12:00 (exatos 180 min)
- Almoço: 12:00 – 13:00
- Tarde: 13:00 – entre 16:00 e 17:00 (mínimo 180 min, máximo 240 min)
- Networking: inicia imediatamente após o fim da sessão da tarde

## Recursos
- Leitura de arquivo CSV de input com título e duração
- Suporte a duração em `XXmin` ou `relâmpago`
- Geração de agenda formatada em texto
- Help automático com `-h` ou `-help`
- Entrada padrão: `sample_input.csv` no diretório de execução

## Requisitos
- .NET SDK 10.0 instalado
- Windows, Linux ou macOS compatível com .NET 10

## Como clonar o projeto
```bash
git clone https://github.com/cleversonlpf/ConferenceScheduler.git
cd ConferenceScheduler
```

## Executar a partir do código-fonte
1. Restaurar pacotes:
```bash
dotnet restore src/ConferenceScheduler.csproj
```
2. Executar usando o arquivo padrão ou um arquivo específico:
```bash
dotnet run --project src/ConferenceScheduler.csproj -- sample_input.csv
```
3. Ou informar um arquivo de entrada e saída explicitamente:
```bash
dotnet run --project src/ConferenceScheduler.csproj -- data/sample_input.csv Agenda.txt
```
4. Mostrar ajuda:
```bash
dotnet run --project src/ConferenceScheduler.csproj -- -h
```

## Observações de uso
- Se não for passado nenhum argumento, o app tenta usar `sample_input.csv` no diretório de execução.
- Se `sample_input.csv` não existir e nenhum argumento for informado, o programa exibirá o help.
- Para usar o sample fornecido na pasta `data`, execute com o caminho `data/sample_input.csv`.

## Formato de entrada
O arquivo de entrada deve ser CSV com duas colunas:
- título da palestra
- duração em minutos ou `relâmpago`

Exemplo:
```csv
Writing Fast Tests Against Enterprise Rails,60min
Overdoing it in Python,45min
Lua for the Masses,30min
Ruby Errors from Mismatched Gem Versions,45min
Common Ruby Errors,45min
Rails for Python Developers,5min
```

## Alternativa: executar via ZIP com arquivo EXE
Se você distribuir uma versão empacotada em ZIP, inclua:
- `ConferenceScheduler.exe`
- `sample_input.csv`
- `README.md`

Passos para executar:
1. Descompacte o arquivo ZIP em uma pasta local.
2. Copie o `sample_input.csv` para a mesma pasta ou use o caminho completo.
3. Execute no terminal:
```powershell
ConferenceScheduler.exe sample_input.csv
```
4. Opcionalmente informe um arquivo de saída:
```powershell
ConferenceScheduler.exe sample_input.csv Agenda.txt
```

## Testes
Para executar a suíte de testes do projeto:
```bash
dotnet test tests/ConferenceScheduler.Tests/ConferenceScheduler.Tests.csproj
```

## Estrutura do projeto
- `src/ConferenceScheduler.csproj` - projeto principal
- `src/Program.cs` - ponto de entrada
- `src/Services/` - parsing, agendamento e formatação
- `src/Model/` - modelos de `Talk`, `Session` e `Track`
- `tests/ConferenceScheduler.Tests/` - testes de unidade
- `data/sample_input.csv` - exemplo de entrada

## Observações finais
Use `sample_input.csv` como entrada padrão para testes rápidos, e personalize o `outputPath` quando desejar gerar um arquivo diferente.
