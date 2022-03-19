#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dash.Models;

namespace Dash.Controllers
{
    public class VendasController : Controller
    {
        private readonly DemoContext _context;

        public VendasController(DemoContext context)
        {
            _context = context;
        }

        // GET: Vendas
        public async Task<IActionResult> Index()
        {
            var demoContext = _context.Vendas.Include(v => v.IdCarroNavigation).Include(v => v.IdClienteNavigation).Include(v => v.IdVendedorNavigation);
            return View(await demoContext.ToListAsync());
        }

        // GET: Vendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendas = await _context.Vendas
                .Include(v => v.IdCarroNavigation)
                .Include(v => v.IdClienteNavigation)
                .Include(v => v.IdVendedorNavigation)
                .FirstOrDefaultAsync(m => m.IdVenda == id);
            if (vendas == null)
            {
                return NotFound();
            }

            return View(vendas);
        }

        // GET: Vendas/Create
        public IActionResult Create()
        {
            ViewData["IdCarro"] = new SelectList(_context.Carros, "Id", "Cor");
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Cidade");
            ViewData["IdVendedor"] = new SelectList(_context.Vendedores, "Id", "Area");
            return View();
        }

        // POST: Vendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdVenda,IdCarro,IdCliente,IdVendedor,FormaPagamento,StatusVenda,ValorVenda")] Vendas vendas)
        {
            if (ModelState.IsValid)
            {
                vendas.DataVenda = DateTime.Now;
                _context.Add(vendas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCarro"] = new SelectList(_context.Carros, "Id", "Cor", vendas.IdCarro);
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Cidade", vendas.IdCliente);
            ViewData["IdVendedor"] = new SelectList(_context.Vendedores, "Id", "Area", vendas.IdVendedor);
            return View(vendas);
        }

        // GET: Vendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendas = await _context.Vendas.FindAsync(id);
            if (vendas == null)
            {
                return NotFound();
            }
            ViewData["IdCarro"] = new SelectList(_context.Carros, "Id", "Cor", vendas.IdCarro);
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Cidade", vendas.IdCliente);
            ViewData["IdVendedor"] = new SelectList(_context.Vendedores, "Id", "Area", vendas.IdVendedor);
            return View(vendas);
        }

        // POST: Vendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdVenda,IdCarro,IdCliente,IdVendedor,DataVenda,FormaPagamento,StatusVenda,ValorVenda")] Vendas vendas)
        {
            if (id != vendas.IdVenda)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendasExists(vendas.IdVenda))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCarro"] = new SelectList(_context.Carros, "Id", "Cor", vendas.IdCarro);
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Cidade", vendas.IdCliente);
            ViewData["IdVendedor"] = new SelectList(_context.Vendedores, "Id", "Area", vendas.IdVendedor);
            return View(vendas);
        }

        // GET: Vendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendas = await _context.Vendas
                .Include(v => v.IdCarroNavigation)
                .Include(v => v.IdClienteNavigation)
                .Include(v => v.IdVendedorNavigation)
                .FirstOrDefaultAsync(m => m.IdVenda == id);
            if (vendas == null)
            {
                return NotFound();
            }

            return View(vendas);
        }

        // POST: Vendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vendas = await _context.Vendas.FindAsync(id);
            _context.Vendas.Remove(vendas);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendasExists(int id)
        {
            return _context.Vendas.Any(e => e.IdVenda == id);
        }
    }
}
