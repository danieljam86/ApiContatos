namespace ApiContatos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ajuste : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Contatos", name: "Endereco_EnderecoId", newName: "EnderecoId");
            RenameIndex(table: "dbo.Contatos", name: "IX_Endereco_EnderecoId", newName: "IX_EnderecoId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Contatos", name: "IX_EnderecoId", newName: "IX_Endereco_EnderecoId");
            RenameColumn(table: "dbo.Contatos", name: "EnderecoId", newName: "Endereco_EnderecoId");
        }
    }
}
