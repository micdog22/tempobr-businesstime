
# TempoBR.BusinessTime (C# • .NET 8)

Biblioteca e API para cálculo de **dias úteis**, **horas úteis** e **feriados brasileiros** (incluindo feriados móveis como Carnaval, Sexta-feira Santa e Corpus Christi), com exportação **ICS**. Útil para prazos, SLAs e agendas.

## Recursos
- Cálculo de feriados nacionais brasileiros, incluindo os móveis a partir da Páscoa.
- Verificação de dia útil e próximo dia útil.
- `add business days` e `add business hours` com janela de trabalho configurável.
- Exportação de calendário **.ics** com feriados para importar em Google/Outlook.
- API REST com Swagger, testes xUnit, Dockerfile, CI com GitHub Actions.
- Extensível para feriados estaduais/municipais.

## Arquitetura
```
tempobr-businesstime/
├─ src/TempoBR.BusinessTime.Core/          # Lógica de calendário
├─ src/TempoBR.BusinessTime.Api/           # API ASP.NET Core
├─ tests/TempoBR.BusinessTime.Tests/       # Testes (xUnit)
├─ Dockerfile
├─ .github/workflows/dotnet.yml
├─ TempoBR.BusinessTime.sln
└─ README.md
```

## Como executar
```bash
dotnet restore
dotnet build
dotnet test

# API
dotnet run --project src/TempoBR.BusinessTime.Api
# Swagger: http://localhost:5199/swagger
```

## Endpoints
- `GET /api/holidays/{year}?uf=SP&city=Sao%20Paulo&includeBlackConsciousness=false`
- `GET /api/business-day/next?date=2025-09-19&uf=SP`
- `GET /api/business-day/add?start=2025-09-19&days=5&uf=SP`
- `POST /api/business-hour/add` body:
  ```json
  { "start": "2025-09-19T16:30:00", "hours": 10, "workStartHour": 9, "workEndHour": 18, "uf": "SP" }
  ```
- `GET /api/ics/holidays/{year}?uf=SP&city=Sao%20Paulo` → baixa um `.ics` (feriados)

## Configuração
- Por padrão, considera feriados **nacionais**. É possível incluir o **Dia da Consciência Negra** via parâmetro.
- Para feriados estaduais/municipais, basta injetar listas adicionais (ex.: carregar JSON) no `HolidayCalculator` (extensão futura).

## Limitações e notas
- `add business hours` respeita apenas uma janela fixa diária (ex.: 9h–18h); jornadas por dia da semana podem ser implementadas conforme necessidade.
- Não há fuso horário persistido; o cálculo usa o `Kind` do `DateTime` recebido.

## Licença
MIT (veja `LICENSE`).
