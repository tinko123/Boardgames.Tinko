using Boardgames.Data.Models;

namespace Boardgames.Data.Models
{
    public class BoardgameSeller
    {
        public int BoardgameId { get; set; }
        public Boardgame Boardgame { get; set; }
        public int SellerId { get; set; }
        public Seller Seller { get; set; }

    }
}
//BoardgameId – integer, Primary Key, foreign key (required)

//· Boardgame – Boardgame

//· SellerId – integer, Primary Key, foreign key (required)

//· Seller – Seller
