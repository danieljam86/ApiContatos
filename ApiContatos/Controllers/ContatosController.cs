using ApiContatos.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;


namespace ApiContatos.Controllers
{
    public class ContatosController : ApiController
    {
        public IHttpActionResult GetTodosContatos(bool incluirEndereco = false)
        {
            IList<Contato> contatos = null;
            using (var ctx = new AppDbContext())
            {
                contatos = ctx.Contatos.Include("Endereco").ToList()
                            .Select(s => new Contato()
                            {
                                ContatoId = s.ContatoId,
                                Nome = s.Nome,
                                Email = s.Email,
                                Telefone = s.Telefone,
                                Endereco = s.Endereco == null || incluirEndereco == false ? null : new Endereco()
                                {
                                    EnderecoId  = s.Endereco.EnderecoId,
                                    Local = s.Endereco.Local,
                                    Cidade = s.Endereco.Cidade,
                                    Estado = s.Endereco.Estado
                                }
                            }).ToList();
            }

            if (contatos.Count == 0)
            {
                return NotFound();
            }
            return Ok(contatos);
        }

        public IHttpActionResult GetContatoPorId(int? id)
        {
            if (id == null)
                return BadRequest("O Id do contato é inválido");

            Contato contato = null;
            using (var ctx = new AppDbContext())
            {
                contato = ctx.Contatos.Include("Endereco").ToList()
                            .Where(c => c.ContatoId == id)
                            .Select(c => new Contato()
                            {
                                ContatoId = c.ContatoId,
                                Nome = c.Nome,
                                Email = c.Email,
                                Telefone = c.Telefone,
                                Endereco = c.Endereco == null ? null : new Endereco()
                                {
                                    EnderecoId = c.Endereco.EnderecoId,
                                    Local = c.Endereco.Local,
                                    Cidade = c.Endereco.Cidade,
                                    Estado = c.Endereco.Estado
                                }
                            }).FirstOrDefault();
            }

            if (contato == null)
            {
                return NotFound();
            }
            return Ok(contato);
        }

        public IHttpActionResult GetContatoPorNome(string nome)
        {
            if (nome == null)
                return BadRequest("Nome Inválido");

            IList<Contato> contatos = null;

            using (var ctx = new AppDbContext())
            {
                contatos = ctx.Contatos.Include("Endereco").ToList()
                    .Where(s => s.Nome.StartsWith(nome))
                    .Select(s => new Contato()
                    {
                        ContatoId = s.ContatoId,
                        Nome = s.Nome,
                        Email = s.Email,
                        Telefone = s.Telefone,
                        Endereco = s.Endereco == null ? null : new Endereco()
                        {
                            EnderecoId = s.Endereco.EnderecoId,
                            Local = s.Endereco.Local,
                            Cidade = s.Endereco.Cidade,
                            Estado = s.Endereco.Estado
                        }
                    }).ToList();
            }

            if (contatos.Count == 0)
            {
                return NotFound();
            }
            return Ok(contatos);
        }
                
        public IHttpActionResult PostNovoContato(ContatoEnderecoDTO contato)
        {
            if (!ModelState.IsValid || contato == null)
                return BadRequest("Os dados do contato são inválidos.");

            using (var ctx = new AppDbContext())
            {
                ctx.Contatos.Add(new Contato()
                {
                    Nome = contato.Nome,
                    Email = contato.Email,
                    Telefone = contato.Telefone,
                    Endereco = new Endereco()
                    {
                        Local = contato.Local,
                        Cidade = contato.Cidade,
                        Estado = contato.Estado
                    }
                });
                ctx.SaveChanges();
            }
            return Ok(contato);
        }

        public IHttpActionResult Put(Contato contato)
        {
            if (!ModelState.IsValid || contato == null )
                  return BadRequest("Os dados do contato são inválidos");

            using (var ctx = new AppDbContext())
            {
                var contatoSelecionado = ctx.Contatos.Where(c => c.ContatoId == contato.ContatoId)
                                         .FirstOrDefault<Contato>();

                if (contatoSelecionado != null)
                {
                    contatoSelecionado.Nome = contato.Nome;
                    contatoSelecionado.Email = contato.Email;
                    contatoSelecionado.Telefone = contato.Telefone;

                    ctx.Entry(contatoSelecionado).State = EntityState.Modified;

                    var enderecoSelecionado = ctx.Enderecos.Where(e =>
                                              e.EnderecoId == contatoSelecionado.Endereco.EnderecoId)
                                              .FirstOrDefault<Endereco>();

                    if (enderecoSelecionado != null)
                    {
                        enderecoSelecionado.Local = contato.Endereco.Local;
                        enderecoSelecionado.Cidade = contato.Endereco.Cidade;
                        enderecoSelecionado.Estado = contato.Endereco.Estado;

                        ctx.Entry(enderecoSelecionado).State = EntityState.Modified;
                    }

                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok($"Contato {contato.Nome} atualizado com sucesso");
        }

        public IHttpActionResult Delete(int? id)
        {
            if (id==null)
                return BadRequest("O id do contato é inválido");

            using (var ctx = new AppDbContext())
            {
                var contatoSelecionado = ctx.Contatos.Where(c => c.ContatoId == id)
                                         .FirstOrDefault<Contato>();

                if (contatoSelecionado != null)
                {
                    ctx.Entry(contatoSelecionado).State = EntityState.Deleted;

                    var enderecoSelecionado = ctx.Enderecos.Where(e =>
                                              e.EnderecoId == contatoSelecionado.EnderecoId)
                                             .FirstOrDefault<Endereco>();

                    if (enderecoSelecionado != null)
                    {
                        ctx.Entry(enderecoSelecionado).State = EntityState.Deleted;
                    }
                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok($"Contato {id} foi deletado com sucesso");
        }
    }
}
