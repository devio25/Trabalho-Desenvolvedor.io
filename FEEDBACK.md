# Feedback - Avaliação Geral

## Front End

### Navegação
  * Pontos positivos:
    - Projeto MVC com rotas completas para cadastro, edição, exclusão e visualização de produtos e categorias.
    - Autenticação e páginas de login/registro implementadas.

  * Pontos negativos:
    - Nenhum.

### Design
  - Interface funcional, aderente ao estilo de um painel administrativo com organização visual clara.

### Funcionalidade
  * Pontos positivos:
    - CRUD de produtos e categorias implementado nas camadas MVC e API.
    - Registro do usuário cria também o vendedor com o mesmo ID.
    - Identity implementado corretamente com cookies no MVC e JWT na API.
    - Migrations automáticas e seed de dados presentes e funcionais.
    - Uso de SQLite devidamente configurado.

  * Pontos negativos:
    - Nenhum.

## Back End

### Arquitetura
  * Pontos positivos:
    - Três projetos distintos bem organizados: MVC, API e biblioteca de Core (`Library`).
    - Separação de responsabilidades clara e uso adequado de dependências.

  * Pontos negativos:
    - `JwtSettings` está junto das entidades, o que não segue a boa prática de separação de configuração e negócios.
    - `Program.cs` está um pouco poluída e poderia se beneficiar da extração de configurações para classes de extensão (abstrações).

### Funcionalidade
  * Pontos positivos:
    - Todas as funcionalidades principais foram implementadas conforme o escopo.

  * Pontos negativos:
    - Nenhum.

### Modelagem
  * Pontos positivos:
    - Entidades modeladas corretamente, com uso coerente de validações.

  * Pontos negativos:
    - Nenhum.

## Projeto

### Organização
  * Pontos positivos:
    - Estrutura de pastas clara, com divisão entre API, MVC e biblioteca compartilhada.
    - Uso do SQLite, seed, migrations e organização de views.

  * Pontos negativos:
    - Arquivo `.sln` está na subpasta `TrabalhoLojaVirtual` em vez da raiz.
    - Arquivos de documentação estão com extensão incorreta `.mb`, o correto seria `.md`.

### Documentação
  * Pontos positivos:
    - Documentação presente com informações de projeto.

  * Pontos negativos:
    - Extensão incorreta dos arquivos `README.mb` e `FEEDBACK.mb`.

### Instalação
  * Pontos positivos:
    - Projeto roda com SQLite e aplica seed/migrations automaticamente.

  * Pontos negativos:
    - Nenhum.

---

# 📊 Matriz de Avaliação de Projetos

| **Critério**                   | **Peso** | **Nota** | **Resultado Ponderado**                  |
|-------------------------------|----------|----------|------------------------------------------|
| **Funcionalidade**            | 30%      | 10       | 3,0                                      |
| **Qualidade do Código**       | 20%      | 9        | 1,8                                      |
| **Eficiência e Desempenho**   | 20%      | 9        | 1,8                                      |
| **Inovação e Diferenciais**   | 10%      | 10       | 1,0                                      |
| **Documentação e Organização**| 10%      | 9        | 0,9                                      |
| **Resolução de Feedbacks**    | 10%      | 10       | 1,0                                      |
| **Total**                     | 100%     | -        | **9,5**                                  |

## 🎯 **Nota Final: 9,5 / 10**
