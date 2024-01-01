using Cards.API.Data;
using Cards.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cards.API.Controllers
{
	[ApiController]
	[Route("api/[Controller]")]
	public class CardsController : Controller
	{
		private readonly CardsDbContext cardsDbContext;

		public CardsController(CardsDbContext cardsDbContext)
        {
			this.cardsDbContext = cardsDbContext;
        }

		[HttpGet]
        public async Task<IActionResult> GetAllCards()
		{
			var cards = await cardsDbContext.Cards.ToListAsync();
			return Ok(cards);
		}

		[HttpGet]
		[Route("{id:guid}")]
		[ActionName("GetCard")]
		public async Task<IActionResult> GetCard([FromRoute] Guid id)
		{
			var card = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
			if(card != null)
			{
				return Ok(card);
			}
			return NotFound("Card not found");
		}

		[HttpPost]
		public async Task<IActionResult> AddCard([FromBody] Card card)
		{
			card.Id = Guid.NewGuid();
			await cardsDbContext.Cards.AddAsync(card);
			await cardsDbContext.SaveChangesAsync();
			return CreatedAtAction(nameof(GetCard), new { id = card.Id }, card);
		}

		[HttpPut]
		[Route("{id:guid}")]
		public async Task<IActionResult> UpdateCard([FromRoute] Guid id, [FromBody] Card card)
		{
			var existingcard = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
			if (existingcard != null)
			{
				existingcard.CardHolderName = card.CardHolderName;
				existingcard.CardNumber = card.CardNumber;
				existingcard.ExpiryMonth = card.ExpiryMonth;
				existingcard.ExpiryYear = card.ExpiryYear;
				existingcard.CVV = card.CVV;
				await cardsDbContext.SaveChangesAsync();
				return Ok(existingcard);
			}
			else
			{
				return NotFound("Card not found");
			}
		}

		[HttpDelete]
		[Route("{id:guid}")]
		public async Task<IActionResult> DeleteCard([FromRoute] Guid id)
		{
			var existingcard = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
			if (existingcard != null)
			{
				cardsDbContext.Remove(existingcard);
				await cardsDbContext.SaveChangesAsync();
				return Ok(existingcard);
			}
			else
			{
				return NotFound("Card not found");
			}
		}
	}
}
