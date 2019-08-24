using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Loja.Modelos;
using SecureApp;

namespace Loja.DAL
{
    public class ProdutosDAL
    {
        public ArrayList ProdutosEmFalta()
        {

            SqlConnection cn = new SqlConnection();
            
            DTICrypto objCrypto = new DTICrypto();
            cn.ConnectionString = objCrypto.Decifrar(Dados.StringDeConexao, "camacho2008");

            SqlCommand cmd = new SqlCommand("Select * from Produtos Where Estoque < 10", cn);

            cn.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            ArrayList lista = new ArrayList();
            while (dr.Read())
            {
                ProdutoInformation produto = new ProdutoInformation();
                produto.Codigo = Convert.ToInt32(dr["codigo"]);
                produto.Nome = dr["nome"].ToString();
                produto.Estoque = Convert.ToInt32(dr["estoque"]);
                produto.Preco = Convert.ToDecimal(dr["preco"]);
                lista.Add(produto);

            }

            dr.Close();
            cn.Close();

            return lista;
        }

        public void Incluir(ProdutoInformation produto)
        {
            //conexao
            SqlConnection cn = new SqlConnection();
            try
            {
                DTICrypto objCrypto = new DTICrypto();
                cn.ConnectionString = objCrypto.Decifrar(Dados.StringDeConexao, "camacho2008");

                //command
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = "insert into Produtos(nome,preco,estoque) values (@nome,@preco,@estoque); select @@IDENTITY;";

                cmd.Parameters.AddWithValue("@nome", produto.Nome);
                cmd.Parameters.AddWithValue("@preco", produto.Preco);
                cmd.Parameters.AddWithValue("@estoque", produto.Estoque);

                cn.Open();
                produto.Codigo = Convert.ToInt32(cmd.ExecuteScalar());

            }
            catch (SqlException ex)
            {
                throw new Exception("Servidor SQL Erro: " + ex.Number);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cn.Close();
            }

        }

        public void Alterar(ProdutoInformation produto)
        {
            //conexao
            SqlConnection cn = new SqlConnection();
            try
            {

                DTICrypto objCrypto = new DTICrypto();
                cn.ConnectionString = objCrypto.Decifrar(Dados.StringDeConexao, "camacho2008");

                //command
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update Produtos set nome = @nome, preco = @preco, estoque = @estoque where codigo = @codigo;";
                cmd.Parameters.AddWithValue("@codigo", produto.Codigo);
                cmd.Parameters.AddWithValue("@nome", produto.Nome);
                cmd.Parameters.AddWithValue("@preco", produto.Preco);
                cmd.Parameters.AddWithValue("@estoque", produto.Estoque);
                
                cn.Open();
                cmd.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                throw new Exception("Servidor SQL Erro: " + ex.Number);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cn.Close();
            }

        }

        public void Excluir(int codigo)
        {
            //conexao
            SqlConnection cn = new SqlConnection();
            try
            {

                DTICrypto objCrypto = new DTICrypto();
                cn.ConnectionString = objCrypto.Decifrar(Dados.StringDeConexao, "camacho2008");

                //command
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = "delete from Produtos where codigo = " + codigo;

                cn.Open();
                int resultado = cmd.ExecuteNonQuery();
                if (resultado != 1)
                {
                    throw new Exception("N�o foi poss�vel excluir o produto " + codigo);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("SQL Erro:" + ex.Number);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }

        public DataTable Listagem(string filtro)
        {
            DataTable tabela = new DataTable();
            string strSql;
            if (filtro == "")
            {
                strSql = "select * from produtos";
            }
            else
            {
                strSql = "select * from produtos where nome like '%" + filtro + "%'";
            }

            DTICrypto objCrypto = new DTICrypto();

            SqlDataAdapter da = new SqlDataAdapter(strSql, objCrypto.Decifrar(Dados.StringDeConexao, "camacho2008"));
            da.Fill(tabela);
            return tabela;
        }

    }
}
