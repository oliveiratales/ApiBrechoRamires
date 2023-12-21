﻿// <auto-generated />
using System;
using ApiBrechoRamires.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ApiBrechoRamires.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231221211809_CreatingLoginTable")]
    partial class CreatingLoginTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ApiBrechoRamires.Models.ProdutoModel", b =>
                {
                    b.Property<string>("Codigo")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Categoria")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Cor")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Dono")
                        .HasColumnType("longtext");

                    b.Property<string>("Marca")
                        .HasColumnType("longtext");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Origem")
                        .HasColumnType("int");

                    b.Property<decimal>("Preco")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal?>("PrecoPago")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int");

                    b.Property<string>("Tamanho")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.Property<int?>("VendaModelId")
                        .HasColumnType("int");

                    b.HasKey("Codigo");

                    b.HasIndex("VendaModelId");

                    b.ToTable("Produtos");
                });

            modelBuilder.Entity("ApiBrechoRamires.Models.VendaModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Cliente")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DataVenda")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal?>("Desconto")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("FormaDePagamento")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("Vendedor")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Vendas");
                });

            modelBuilder.Entity("ApiBrechoRamires.Models.VendaProduto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ProdutoCodigo")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProdutoModelCodigo")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int");

                    b.Property<int>("VendaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProdutoCodigo");

                    b.HasIndex("ProdutoModelCodigo");

                    b.HasIndex("VendaId");

                    b.ToTable("VendaProdutos");
                });

            modelBuilder.Entity("ApiBrechoRamires.Models.ProdutoModel", b =>
                {
                    b.HasOne("ApiBrechoRamires.Models.VendaModel", null)
                        .WithMany("ProdutosVendidos")
                        .HasForeignKey("VendaModelId");
                });

            modelBuilder.Entity("ApiBrechoRamires.Models.VendaProduto", b =>
                {
                    b.HasOne("ApiBrechoRamires.Models.ProdutoModel", "Produto")
                        .WithMany("VendaProdutos")
                        .HasForeignKey("ProdutoCodigo")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ApiBrechoRamires.Models.ProdutoModel", null)
                        .WithMany("VendasAssociadas")
                        .HasForeignKey("ProdutoModelCodigo");

                    b.HasOne("ApiBrechoRamires.Models.VendaModel", "Venda")
                        .WithMany("VendaProdutos")
                        .HasForeignKey("VendaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Produto");

                    b.Navigation("Venda");
                });

            modelBuilder.Entity("ApiBrechoRamires.Models.ProdutoModel", b =>
                {
                    b.Navigation("VendaProdutos");

                    b.Navigation("VendasAssociadas");
                });

            modelBuilder.Entity("ApiBrechoRamires.Models.VendaModel", b =>
                {
                    b.Navigation("ProdutosVendidos");

                    b.Navigation("VendaProdutos");
                });
#pragma warning restore 612, 618
        }
    }
}
