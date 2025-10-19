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


