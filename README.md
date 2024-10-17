### 💻 Sobre
Projeto de estudos desenvolvido com base na formação da Alura: Testes em .NET, testando integração com banco de dados. 

Neste projeto foi criado testes de integração de baixo acoplamento com base na biblioteca XUnit, aplicando soluções de compartilhamento de conexão do banco de dados.
Utilizando o Docker para termos um ambiente mais controlado e otimizado durante os testes e também o uso de massa de dados para os testes de integração, utilizando a biblioteca Bogos.

## 🔨 Principais técnicas e padrões empregadas no projeto
- [x] Testes de integração de baixo acoplamento isolando a conexão com o banco de dados para reaproveita-la em testes diferentes.
- [x] Utilizando ClassFixture em conjunto com CollectionFixture para o compartilhamento de conexões entre as classes de teste.
- [x] Fazendo uso do Docker para configurar um ambiente de banco de dados para testes de integração.
- [x] Aplicando comunicações assíncronas através da interface IAsyncLifetime.
- [x] Utilizando migrations do EntityFramework para configurar o banco de testes e suas tabelas.
- [x] Insrindo massa de dados através da biblioteca Bogus para popular o banco de dados de teste.
- [x] Implementando o padrão de projeto data builder para construir objetos para os testes.
- [x] Controlando os dados que serão inicializados e finalizados durante os testes (com teardown).
