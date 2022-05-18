namespace ApiContatos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contatos",
                c => new
                    {
                        ContatoId = c.Int(nullable: false, identity: true),
                        Nome = c.String(maxLength: 100),
                        Email = c.String(maxLength: 150),
                        Telefone = c.String(maxLength: 40),
                        Endereco_EnderecoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ContatoId)
                .ForeignKey("dbo.Enderecos", t => t.Endereco_EnderecoId, cascadeDelete: true)
                .Index(t => t.Endereco_EnderecoId);
            
            CreateTable(
                "dbo.Enderecos",
                c => new
                    {
                        EnderecoId = c.Int(nullable: false, identity: true),
                        Local = c.String(maxLength: 150),
                        Cidade = c.String(maxLength: 100),
                        Estado = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.EnderecoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contatos", "Endereco_EnderecoId", "dbo.Enderecos");
            DropIndex("dbo.Contatos", new[] { "Endereco_EnderecoId" });
            DropTable("dbo.Enderecos");
            DropTable("dbo.Contatos");
        }
    }
}
