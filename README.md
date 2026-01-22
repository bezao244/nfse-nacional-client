# NFSe Nacional Client - Cliente .NET para o Sistema Nacional NFS-e

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

Cliente .NET completo e profissional para consumir as APIs do Sistema Nacional NFS-e (Sefin Nacional).

## 📋 Índice

- [Sobre](#sobre)
- [Características](#características)
- [Instalação](#instalação)
- [Configuração](#configuração)
- [Uso](#uso)
  - [Emissão de NFS-e](#emissão-de-nfs-e)
  - [Consulta de NFS-e](#consulta-de-nfs-e)
  - [Cancelamento](#cancelamento)
  - [Consulta de Parâmetros Municipais](#consulta-de-parâmetros-municipais)
  - [Verificação de DPS](#verificação-de-dps)
  - [Eventos](#eventos)
- [APIs Implementadas](#apis-implementadas)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Exemplos](#exemplos)
- [Documentação Oficial](#documentação-oficial)
- [Contribuindo](#contribuindo)
- [Licença](#licença)

## 🎯 Sobre

O Sistema Nacional NFS-e fornece APIs REST para emissão, consulta e gestão de Notas Fiscais de Serviço Eletrônicas. Este cliente implementa o consumo dessas APIs seguindo as especificações técnicas oficiais.

**Ambiente de Produção Restrita (Homologação):** `https://adn.producaorestrita.nfse.gov.br/contribuintes`

## ✨ Características

- ✅ **Autenticação com Certificado Digital** - Suporte para certificados A1 e A3
- ✅ **Serialização/Deserialização XML** - Conversão automática entre objetos .NET e XML
- ✅ **Tratamento de Erros** - Exceptions customizadas e mensagens claras
- ✅ **Validação de Dados** - Validações client-side antes do envio
- ✅ **Documentação Completa** - Comentários XML em todos os membros públicos
- ✅ **Exemplos Práticos** - Exemplos funcionais para cada serviço
- ✅ **Configuração Flexível** - URLs e timeouts configuráveis
- ✅ **Suporte a Assinatura Digital** - Estrutura base para xmldsig

## 📦 Instalação

### Requisitos

- .NET 8.0 ou superior
- Certificado Digital A1 ou A3

### Via Código Fonte

```bash
git clone https://github.com/bezao244/nfse-nacional-client.git
cd nfse-nacional-client/NFSeNacional.Client
dotnet restore
dotnet build
```

### Adicionando ao Seu Projeto

```bash
# No diretório do seu projeto
dotnet add reference /caminho/para/NFSeNacional.Client/NFSeNacional.Client.csproj
```

## ⚙️ Configuração

### Configuração Básica

```csharp
using System.Security.Cryptography.X509Certificates;
using NFSeNacional.Client.Configuration;

// Carregar certificado digital
var certificado = new X509Certificate2(
    "/caminho/para/certificado.pfx",
    "senha_do_certificado"
);

// Criar configuração
var config = new NFSeConfig
{
    BaseUrl = "https://adn.producaorestrita.nfse.gov.br/contribuintes",
    Ambiente = 2, // 1=Produção, 2=Homologação
    CertificadoDigital = certificado,
    TimeoutSegundos = 30,
    VersaoAplicativo = "1.01",
    ValidarCertificadoServidor = false // Para homologação
};

// Validar configuração
if (!config.IsValid())
{
    var erros = config.GetValidationErrors();
    throw new Exception($"Configuração inválida: {string.Join(", ", erros)}");
}
```

## 🚀 Uso

### Emissão de NFS-e

```csharp
using NFSeNacional.Client.Services;
using NFSeNacional.Client.Models;

// Criar DPS (Declaração de Prestação de Serviços)
var dps = new DPS
{
    InfDps = new InfDPS
    {
        Id = "3550308212345678901234123450000000000001", // 42 caracteres
        Prestador = new Prestador
        {
            CNPJ = "12345678901234",
            InscricaoMunicipal = "123456",
            CodigoMunicipio = "3550308" // São Paulo
        },
        Tomador = new Tomador
        {
            TipoInscricao = TipoInscricao.CNPJ,
            NumeroInscricao = "98765432109876",
            Nome = "Empresa Tomadora Ltda",
            Endereco = new Endereco
            {
                Logradouro = "Rua Exemplo",
                Numero = "123",
                Bairro = "Centro",
                CodigoMunicipio = "3550308",
                CEP = "01000000"
            },
            Email = "contato@empresatomadora.com.br"
        },
        Servico = new Servico
        {
            CodigoServico = "01.01",
            CodigoCNAE = "6201500",
            Discriminacao = "Desenvolvimento de software sob encomenda",
            CodigoMunicipioIncidencia = "3550308"
        },
        Valores = new ValoresDPS
        {
            ValorServicos = 1000.00m,
            ValorDeducoes = 0.00m,
            ValorBaseCalculo = 1000.00m,
            Aliquota = 5.00m,
            ValorISS = 50.00m,
            ValorLiquido = 950.00m,
            IndicadorIncentivo = IndicadorIncentivo.Nao
        },
        NumeroDPS = "000000000000001",
        Serie = "00001",
        DataHoraEmissao = DateTime.Now,
        Competencia = DateTime.Now,
        NaturezaOperacao = NaturezaOperacao.TributacaoNoMunicipio
    }
};

// Emitir NFS-e
using var service = new NFSeService(config);
var resposta = await service.EmitirNFSeAsync(dps);

if (resposta.Status == "sucesso" && resposta.NFSe != null)
{
    Console.WriteLine($"NFS-e emitida com sucesso!");
    Console.WriteLine($"Número: {resposta.NFSe.InfNfse?.NumeroNFSe}");
    Console.WriteLine($"Chave: {resposta.NFSe.InfNfse?.ChaveAcesso}");
}
```

### Consulta de NFS-e

```csharp
using var service = new NFSeService(config);

var chaveAcesso = "12345678901234567890123456789012345678901234567890"; // 50 caracteres

try
{
    var nfse = await service.ConsultarNFSeAsync(chaveAcesso);
    
    Console.WriteLine($"NFS-e encontrada:");
    Console.WriteLine($"Número: {nfse.InfNfse?.NumeroNFSe}");
    Console.WriteLine($"Emissão: {nfse.InfNfse?.DataHoraEmissao}");
    Console.WriteLine($"Valor: R$ {nfse.InfNfse?.Valores?.ValorLiquido}");
}
catch (NFSeException ex)
{
    Console.WriteLine($"Erro ao consultar NFS-e: {ex.Message}");
}
```

### Cancelamento

```csharp
using var service = new EventosService(config);

var chaveAcesso = "12345678901234567890123456789012345678901234567890"; // 50 caracteres

// Criar pedido de cancelamento
var pedido = service.CriarPedidoCancelamento(
    chaveAcesso,
    "001", // Código de cancelamento
    "Erro na emissão - dados incorretos"
);

var resposta = await service.RegistrarEventoAsync(chaveAcesso, pedido);

if (resposta.Status == "sucesso")
{
    Console.WriteLine("NFS-e cancelada com sucesso!");
}
```

### Consulta de Parâmetros Municipais

```csharp
using var service = new ParametrosMunicipaisService(config);

// Consultar convênio de São Paulo
var convenio = await service.ConsultarConvenioAsync("3550308");
Console.WriteLine($"Município: {convenio.CodigoMunicipio}");
Console.WriteLine($"Situação Convênio: {convenio.SituacaoConvenio}");

// Consultar alíquota de um serviço específico
var servico = await service.ConsultarAliquotasServicoAsync("3550308", "01.01");
Console.WriteLine($"Serviço: {servico.DescricaoServico}");
Console.WriteLine($"Alíquota: {servico.Aliquota}%");
```

### Verificação de DPS

```csharp
using var service = new DPSService(config);

// Construir ID da DPS
var idDps = DPSService.ConstruirIdDps(
    codigoMunicipio: "3550308",
    tipoInscricao: "2", // CNPJ
    inscricaoFederal: "12345678901234",
    serieDps: "00001",
    numeroDps: "000000000000001"
);

// Verificar se NFS-e foi gerada
var gerada = await service.VerificarNFSeGeradaAsync(idDps);

if (gerada)
{
    // Recuperar chave de acesso
    var chaveAcesso = await service.RecuperarChaveAcessoAsync(idDps);
    Console.WriteLine($"Chave de acesso: {chaveAcesso}");
}
```

### Eventos

```csharp
using var service = new EventosService(config);

var chaveAcesso = "12345678901234567890123456789012345678901234567890"; // 50 caracteres

// Consultar todos os eventos
var eventos = await service.ConsultarEventosAsync(chaveAcesso);

foreach (var evento in eventos.Eventos ?? new List<Evento>())
{
    Console.WriteLine($"Tipo: {evento.InfEvento?.TipoEvento}");
    Console.WriteLine($"Data: {evento.InfEvento?.DataHoraEvento}");
}

// Consultar apenas eventos de cancelamento
var cancelamentos = await service.ConsultarEventosPorTipoAsync(chaveAcesso, "e101101");

// Confirmar operação pelo tomador
var pedido = service.CriarPedidoConfirmacao(chaveAcesso, "tomador");
var resposta = await service.RegistrarEventoAsync(chaveAcesso, pedido);
```

## 🔌 APIs Implementadas

### 1. API Parâmetros Municipais

- ✅ `GET /parametros_municipais/{codigoMunicipio}/convenio` - Consulta parâmetros do convênio
- ✅ `GET /parametros_municipais/{codigoMunicipio}/{codigoServico}` - Consulta alíquotas e regimes especiais
- ✅ `GET /parametros_municipais/{codigoMunicipio}/{CPF/CNPJ}/retencoes` - Consulta retenções do contribuinte
- ✅ `GET /parametros_municipais/{codigoMunicipio}/{CPF/CNPJ}/beneficios` - Consulta benefícios municipais

### 2. API NFS-e

- ✅ `POST /nfse` - Geração síncrona de NFS-e a partir de DPS (XML)
- ✅ `GET /nfse/{chaveAcesso}` - Consulta NFS-e pela chave de acesso (50 caracteres)

### 3. API DPS

- ✅ `GET /dps/{id}` - Recupera chave de acesso da NFS-e pelo ID da DPS (42 caracteres)
- ✅ `HEAD /dps/{id}` - Verifica se NFS-e foi gerada a partir da DPS

### 4. API Eventos

- ✅ `POST /nfse/{chaveAcesso}/eventos` - Registra evento (cancelamento, confirmação, rejeição, etc.)
- ✅ `GET /nfse/{chaveAcesso}/eventos` - Consulta todos os eventos de uma NFS-e
- ✅ `GET /nfse/{chaveAcesso}/eventos/{tipoEvento}` - Consulta eventos por tipo
- ✅ `GET /nfse/{chaveAcesso}/eventos/{tipoEvento}/{numSeqEvento}` - Consulta evento específico

## 📁 Estrutura do Projeto

```
NFSeNacional.Client/
├── Configuration/
│   └── NFSeConfig.cs           # Configurações de conexão
├── Models/
│   ├── DPS.cs                  # Declaração de Prestação de Serviços
│   ├── NFSe.cs                 # Nota Fiscal de Serviço Eletrônica
│   ├── Evento.cs               # Eventos de NFS-e
│   ├── ParametrosMunicipais.cs # Parâmetros municipais
│   └── Enums.cs                # Enumerações
├── Services/
│   ├── NFSeHttpClient.cs       # Cliente HTTP base
│   ├── ParametrosMunicipaisService.cs
│   ├── NFSeService.cs
│   ├── DPSService.cs
│   └── EventosService.cs
├── Examples/
│   └── ExemplosDeUso.cs        # Exemplos práticos
└── NFSeException.cs            # Exception customizada
```

## 📚 Exemplos

Veja o arquivo [ExemplosDeUso.cs](NFSeNacional.Client/Examples/ExemplosDeUso.cs) para exemplos completos de:

1. Configuração inicial do cliente
2. Emissão de NFS-e a partir de DPS
3. Consulta de NFS-e pela chave de acesso
4. Cancelamento de NFS-e
5. Consulta de parâmetros municipais
6. Verificação de DPS
7. Consulta de eventos de uma NFS-e
8. Confirmação de operação pelo tomador

## 📖 Documentação Oficial

- [API Produção Restrita (Homologação)](https://adn.producaorestrita.nfse.gov.br/contribuintes/docs/index.html)
- Schemas XSD oficiais (incluídos no projeto)

## 🤝 Contribuindo

Contribuições são bem-vindas! Por favor:

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/NovaFuncionalidade`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova funcionalidade'`)
4. Push para a branch (`git push origin feature/NovaFuncionalidade`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## ⚠️ Avisos Importantes

- **Assinatura Digital:** A implementação da assinatura XML deve seguir o padrão xmldsig. A estrutura base está presente, mas a implementação completa da assinatura deve ser adicionada conforme necessário.
- **Validações:** O sistema nacional realiza validações server-side. Validações client-side básicas estão implementadas.
- **Chaves de Acesso:** Formatos específicos devem ser validados (50 chars NFS-e, 42 chars DPS).
- **Ambiente:** Comece com foco em homologação. Teste exaustivamente antes de usar em produção.
- **Certificados:** Em ambiente de homologação, pode ser necessário desabilitar a validação de certificado do servidor.

## 🔗 Links Úteis

- [Documentação do Sistema Nacional NFS-e](https://www.gov.br/nfse)
- [Portal do Desenvolvedor](https://adn.producaorestrita.nfse.gov.br/contribuintes/docs/)
- [LC 116/2003 - Lista de Serviços](http://www.planalto.gov.br/ccivil_03/leis/lcp/lcp116.htm)

## 📞 Suporte

Para questões relacionadas ao Sistema Nacional NFS-e, consulte a documentação oficial ou entre em contato com o suporte da Sefin Nacional.

Para questões relacionadas a este cliente, abra uma [issue](https://github.com/bezao244/nfse-nacional-client/issues) no GitHub.

---

Desenvolvido com ❤️ para a comunidade brasileira de desenvolvedores.