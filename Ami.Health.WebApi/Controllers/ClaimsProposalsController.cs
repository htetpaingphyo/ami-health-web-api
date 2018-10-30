using Ami.Health.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ami.Health.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("Claims/[action]")]
    public class ClaimsProposalsController : Controller
    {
        private readonly MainDbContext _context;

        // Constructor => Dependency Injected...
        public ClaimsProposalsController(MainDbContext context)
        {
            _context = context;
        }

        // GET: ClaimsProposals
        [HttpGet]
        [ActionName("Get")]
        public IEnumerable<ClaimsProposal> Get()
        {
            return _context.ClaimsProposals;
        }

        // GET: ClaimsProposals/{Base64EncodedId}
        [HttpGet("{id}")]
        [ActionName("Get")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var policy = Encoding.UTF8.GetString(Convert.FromBase64String(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            /***
             * Chage from "SingleOrDefaultAsync()" to "ToListAsync()" with "Where()" cluase 
             * in case of one or more claims will be in a particular policy.
             * **/
            var claimsProposal = await _context.ClaimsProposals.Where(m => m.PolicyNo == policy).ToListAsync();

            if (claimsProposal == null)
            {
                return NotFound();
            }

            return Ok(claimsProposal);
        }

        // POST: ClaimsProposals/Edit
        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> Edit([FromBody] ClaimsViewModel claim)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var upd_claim = await _context.ClaimsProposals
                                .Where(
                                    m => m.PolicyNo == claim.PolicyNo &&
                                    m.ClaimStatus == Core.Entities.ClaimStatus.Pending)
                                .OrderByDescending(m => m.CreatedDate)
                                .FirstOrDefaultAsync();

            if (null == upd_claim)
            {
                return NotFound();
            }

            upd_claim.PolicyNo = claim.PolicyNo;
            upd_claim.InsuredName = claim.InsuredName;
            upd_claim.NIC = claim.NIC;
            upd_claim.Phone = claim.Phone;
            upd_claim.CauseOfLoss = claim.CauseOfLoss;
            upd_claim.NameOfTreatment = claim.NameOfTreatment;
            upd_claim.FromHospitalization = claim.FromHospitalization;
            upd_claim.ToHospitalization = claim.ToHospitalization;
            upd_claim.ReinvestmentAmt = claim.ReinvestmentAmt;
            upd_claim.UpdatedDate = DateTime.Now;

            _context.ClaimsProposals.Update(upd_claim);
            try
            {
                if (await _context.SaveChangesAsync() > 0)
                {
                    return Json(upd_claim);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // POST: ClaimsProposals/Submit
        [HttpPost]
        [ActionName("Submit")]
        public async Task<IActionResult> Submit([FromBody] ClaimsViewModel claim)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if claim proposal's status is in 'PENDING' ???
            var chk_pending = _context.ClaimsProposals
                                .Where(
                                    e => e.PolicyNo == claim.PolicyNo &&
                                    e.ClaimStatus == Core.Entities.ClaimStatus.Pending)
                                .SingleOrDefault();

            if (chk_pending != null)
            {
                return BadRequest();
            }

            // New claim proposal setup...
            var new_claim = new ClaimsProposal()
            {
                Id = new Guid(),
                PolicyNo = claim.PolicyNo,
                InsuredName = claim.InsuredName,
                NIC = claim.NIC,
                Phone = claim.Phone,
                InitimatedDate = DateTime.Now,
                CauseOfLoss = claim.CauseOfLoss,
                NameOfTreatment = claim.NameOfTreatment,
                FromHospitalization = claim.FromHospitalization,
                ToHospitalization = claim.ToHospitalization,
                ClaimStatus = Core.Entities.ClaimStatus.Pending,
                ReinvestmentAmt = claim.ReinvestmentAmt,
                CreatedDate = DateTime.Now,
                UpdatedDate = null
            };

            try
            {
                _context.ClaimsProposals.Add(new_claim);
                await _context.SaveChangesAsync();

                // return CreatedAtAction(nameof(GetClaimsProposal), new { id = new_claim.Id }, new_claim);
                return Json(new_claim);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: ClaimsProposals/Cancel/{Base64EncodedId}
        [HttpGet("{id}")]
        [ActionName("Cancel")]
        public async Task<IActionResult> Cancel([FromRoute] string id)
        {
            var policy = Encoding.UTF8.GetString(Convert.FromBase64String(id));

            var claimsProposal = await _context.ClaimsProposals
                                        .SingleOrDefaultAsync(
                                            m => m.PolicyNo == policy &&
                                            m.ClaimStatus == Core.Entities.ClaimStatus.Pending
                                        );

            if (claimsProposal == null)
            {
                return NotFound();
            }

            claimsProposal.ClaimStatus = Core.Entities.ClaimStatus.CancelledByCustomer;
            claimsProposal.UpdatedDate = DateTime.Now;
            _context.ClaimsProposals.Update(claimsProposal);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}