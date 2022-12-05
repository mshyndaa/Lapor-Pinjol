#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaporPinjol.Data;
using LaporPinjol.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LaporPinjol.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ReportContext _context;

        public ReportsController(ReportContext context)
        {
            _context = context;
        }

        // GET: Reports
        public async Task<IActionResult> Index()
        {
            return View(await _context.Report.ToListAsync());
        }

        // GET: Reports/ShowNameSearchForm
        public IActionResult ShowNameSearchForm()
        {
            return View();
        }

        // GET: Reports/ShowNameSearchResult
        public async Task<IActionResult> ShowNameSearchResult(String SearchPhrase)
        {
            if (!String.IsNullOrWhiteSpace(SearchPhrase))
            {
                var searchName = await _context.Report.Where(i => i.LoanCompanyName.Contains(SearchPhrase)).ToListAsync();
                return View(searchName);
            }

            return NotFound();
        }

        // GET: Reports/ShowBankAccountSearchForm
        public IActionResult ShowBankAccountSearchForm()
        {
            return View();
        }

        // GET: Reports/ShowBankAccountSearchResult
        public async Task<IActionResult> ShowBankAccountSearchResult(String SearchPhrase)
        {
            if (!String.IsNullOrWhiteSpace(SearchPhrase))
            {
                var searchBankAccount = await _context.Report.Where(i => i.LoanCompanyBankAccount.Contains(SearchPhrase)).ToListAsync();
                return View(searchBankAccount);
            }

            return NotFound();
        }

        // GET: Reports/ShowPhoneNumberSearchForm
        public IActionResult ShowPhoneNumberSearchForm()
        {
            return View();
        }

        // GET: Reports/ShowPhoneNumberSearchResult
        public async Task<IActionResult> ShowPhoneNumberSearchResult(String SearchPhrase)
        {
            if (!String.IsNullOrWhiteSpace(SearchPhrase))
            {
                var searchPhoneNumber = await _context.Report.Where(i => i.LoanCompanyPhoneNumber.Contains(SearchPhrase)).ToListAsync();
                return View(searchPhoneNumber);
            }

            return NotFound();
        }

        // GET: Reports/Status
        public async Task<IActionResult> Status()
        {
            return View(await _context.Report.Where(i => i.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).ToListAsync());
        }

        // GET: Reports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Report
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (report == null)
            {
                return NotFound();
            }

            if (report.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return View(report);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Reports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportId,UserId,LoanCompanyName,LoanCompanyBankAccount,LoanCompanyPhoneNumber,EvidenceLinkOnCloudStorage,VerificationStatus,SubmissionDate")] Report report)
        {
            if (ModelState.IsValid)
            {
                Report modifiedReport = new Report
                {
                    ReportId = report.ReportId,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    LoanCompanyName = report.LoanCompanyName,
                    LoanCompanyBankAccount = report.LoanCompanyBankAccount,
                    LoanCompanyPhoneNumber = report.LoanCompanyPhoneNumber,
                    EvidenceLinkOnCloudStorage = report.EvidenceLinkOnCloudStorage,
                    VerificationStatus = ReportStatus.Submitted,
                    SubmissionDate = DateTime.UtcNow
                };
                _context.Add(modifiedReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Status));
            }
            return View(report);
        }

        // GET: Reports/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Report.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            return View(report);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReportId,UserId,LoanCompanyName,LoanCompanyBankAccount,LoanCompanyPhoneNumber,EvidenceLinkOnCloudStorage,VerificationStatus,SubmissionDate")] Report report)
        {
            if (id != report.ReportId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.ReportId))
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
            return View(report);
        }

        // GET: Reports/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Report
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // POST: Reports/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.Report.FindAsync(id);
            _context.Report.Remove(report);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportExists(int id)
        {
            return _context.Report.Any(e => e.ReportId == id);
        }
    }
}
