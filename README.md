# TP01 — Integração de Dados IoT e Meteorológicos (KNIME)

**Autora:** Maria Eduarda Checan (N.º 25499)  
**Unidade Curricular:** Integração de Sistemas de Informação  
**Instituição:** IPCA — Instituto Politécnico do Cávado e do Ave  
**Data:** 19/10/2025

---

## Descrição Geral do Projeto

O presente trabalho tem como objetivo desenvolver uma solução de integração de dados IoT e meteorológicos, recorrendo à ferramenta KNIME Analytics Platform como ambiente ETL.  
A proposta envolve recolher dados simulados de sensores IoT (temperatura, humidade e luminosidade) e cruzá-los com dados meteorológicos provenientes da API OpenWeather.

O processo visa demonstrar a automação completa de um fluxo de integração de dados — desde a recolha e transformação até à exportação e notificação automática.

---

## Descrição dos Ficheiros Enviados

```plaintext
tp01-25499/
│
├── README.md               → Identificação da autora, descrição e instruções de execução
│
├── doc/
│   └── 25499_doc.pdf       → Relatório técnico do projeto
│
├── data/
│   ├── input/              → Dados de entrada 
│   └── output/             → Resultados processados (CSV, XML, estatísticas)
│
├── dataint/                → Workflows KNIME (.knwf)
│   ├── ETL_IoT_Projeto.knwf         → Integração IoT + API 
│
└── src
    └──  AnaliseDadosISI       → Script em C# que gera o resumo diário XML e estatísticas
        ├── AnaliseDadosISI.sln
        ├── Program.cs
        └── GerarXML.cs
```
**Ferramentas Utilizadas**
- KNIME Analytics Platform 5.7 — Plataforma principal de ETL
- MySQL Server — Base de dados de destino para armazenamento
- C# (.NET) — Geração do ficheiro XML com estatísticas
- Node-RED — Simulação de dados IoT em JSON
- Power BI — Visualização dos resultados integrados
- OpenWeather API — Fonte de dados meteorológicos externos



**Como Executar a Solução**



**1. Preparação do Ambiente**
- Instalar KNIME Analytics Platform (v5.7 ou superior)
- Instalar MySQL Server e configurar base de dados iot_db
- Garantir ligação à API OpenWeather (chave gratuita)



**2. Importar e Executar os Jobs no KNIME**
- Abrir o KNIME → File → Import KNIME Workflow...
- Selecionar o ficheiro ETL_IoT_Projeto.knwf da pasta dataint/
- Executar o workflow


O processo irá:
Ler os ficheiros de entrada (data/input/)
Fazer a junção com os dados meteorológicos
Normalizar as datas 
Gerar logs automáticos em CSV
Exportar os resultados em CSV (data/output/)
Passar os dados para a base de dados do MySQL
Enviar email automático


Logs: Criados automaticamente pelo job LOG_Manager
Email de Sucesso: Enviado pelo job SuccessFlow_Email



**3. Executar o Script C#**
Para gerar manualmente o XML de resumo:
cd src
dotnet run GerarXML.cs


(ou abrir o ficheiro no Visual Studio e clicar em "Executar")
O script lê os dados do CSV final e cria resumo_diario.xml em data/output/.


Resultados Gerados
Ficheiros CSV de estatísticas: Dados consolidados prontos para análise
Ficheiro XML: Estatísticas diárias geradas pelo script C#



**Vídeo de Demonstração**
Link do vídeo: https://youtu.be/2ENgcFa5ULI?si=fMj-BPffhrMjQBZ5
QR Code: incluído no relatório PDF



**Observações Finais**
- O fluxo foi totalmente testado com dados simulados
- Todos os componentes foram implementados de forma modular
- A execução é totalmente automatizada — sem intervenção manual
- O projeto demonstra a aplicação prática de conceitos ETL e integração IoT



