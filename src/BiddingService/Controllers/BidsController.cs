﻿using BiddingService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace BiddingService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BidsController : ControllerBase
{
    // [Authorize]
    // [HttpPost]
    // public async Task<ActionResult<Bid>> PlaceBid(string auctionId, int amount)
    // {
    //      var auction = await DB.Find<Auction>().OneAsync(auctionId);

    //      if (auction == null)
    //      {
    //         // TODO: check with auction service if that has auction
    //         return NotFound();
    //      }
    // }
}
