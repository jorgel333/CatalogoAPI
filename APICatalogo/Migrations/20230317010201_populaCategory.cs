﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    public partial class populaCategory : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Categorys(Name, ImageUrl) Values('Bebidas','bebidas.jpg')");
            mb.Sql("Insert into Categorys(Name, ImageUrl) Values('Lanches','lanches.jpg')");
            mb.Sql("Insert into Categorys(Name, ImageUrl) Values('Sobremesas','sobremesas.jpg')");
        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Categorys");
        }
    }
}
