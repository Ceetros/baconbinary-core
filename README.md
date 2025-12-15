# BaconBinary.Core

## Visão Geral

`BaconBinary.Core` é a biblioteca de núcleo do ecossistema de ferramentas BaconBinary. Desenvolvida em C#, ela fornece a lógica central para a manipulação de arquivos de assets do Tibia, especificamente os formatos `.dat` e `.spr`. A biblioteca foi projetada para ser a base compartilhada entre diversas ferramentas, como editores de objetos, mapas e itens, garantindo consistência e reuso de código.

## Arquitetura

A arquitetura do `BaconBinary.Core` é focada em fornecer uma abstração robusta sobre os formatos de arquivo do Tibia. Seus principais componentes são:

-   **`IO` (Entrada/Saída):** Contém a lógica de baixo nível para leitura e escrita dos arquivos `.dat` e `.spr`. Essa camada é responsável por interpretar a estrutura binária dos arquivos e convertê-la para modelos de dados utilizáveis.

-   **`Models`:** Define as estruturas de dados que representam os assets do jogo, como itens, criaturas e sprites. Essas classes servem como a representação em memória dos dados lidos dos arquivos.

-   **`ClientVersionRepository`:** Um componente crucial que gerencia as diferentes versões do cliente Tibia. Ele mantém um registro das assinaturas (signatures) dos arquivos `.dat` e `.spr` para cada versão, permitindo a detecção automática da versão do cliente a partir de um arquivo.

## Funcionalidades

-   **Detecção de Versão:** A biblioteca pode identificar a versão do cliente Tibia a partir de um arquivo `.dat`, lendo sua assinatura e comparando-a com o repositório de versões conhecidas.

-   **Leitura de Assets:** `BaconBinary.Core` é capaz de parsear os arquivos `.dat` e `.spr`, extraindo informações sobre itens, criaturas e outros elementos gráficos do jogo.

-   **Estruturas de Dados:** Oferece um conjunto de modelos bem definidos que representam os assets do Tibia, facilitando a manipulação e a lógica de negócios nas ferramentas que a consomem.

## Exemplo de Uso

A seguir, um exemplo simplificado de como a biblioteca pode ser utilizada para detectar a versão de um cliente a partir de um arquivo `Tibia.dat`:

```csharp
using BaconBinary.Core;
using System;

public class VersionDetector
{
    public void DetectClientVersion(string datFilePath)
    {
        string version = ClientVersionRepository.DetectVersion(datFilePath);

        if (version != null)
        {
            Console.WriteLine($"Versão do cliente detectada: {version}");
        }
        else
        {
            Console.WriteLine("Não foi possível detectar a versão do cliente.");
        }
    }
}
```

## Integração

`BaconBinary.Core` foi projetado para ser consumido por outras aplicações do ecossistema BaconBinary. Ferramentas como o `BaconBinary.ObjectEditor` utilizam esta biblioteca para toda a lógica de manipulação de arquivos, separando as responsabilidades e mantendo a interface do usuário agnóstica em relação aos detalhes de implementação dos formatos de arquivo.
