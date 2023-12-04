namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;
    using Boardgames.Extensions;
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            List<ImportCreatorDto> creatorDtos = xmlString.DeserializeFromXml<List<ImportCreatorDto>>("Creators");
            StringBuilder sb = new StringBuilder();
            List<Creator> creators = new List<Creator>();
            foreach (var creatorDto in creatorDtos)
            {
                if (!IsValid(creatorDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Creator creator = new Creator() 
                {
                    FirstName = creatorDto.FirstName,
                    LastName = creatorDto.LastName
                };
                foreach(var boardgameDto in creatorDto.Boardgames)
                {
                    if(!IsValid(boardgameDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    creator.Boardgames.Add(new Boardgame()
                    {
                        Name = boardgameDto.Name,
                        Rating = boardgameDto.Rating,
                        YearPublished = boardgameDto.YearPublished,
                        CategoryType = (CategoryType) boardgameDto.CategoryType,
                        Mechanics = boardgameDto.Mechanics

                    });
                }
                creators.Add(creator);
                sb.AppendLine(string.Format(SuccessfullyImportedCreator,creator.FirstName,creator.LastName,creator.Boardgames.Count()));
            }
            context.Creators.AddRange(creators);
            context.SaveChanges();
            return sb.ToString();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            var sb = new StringBuilder();
            List<Seller> sellers = new List<Seller>();
            List<ImportSellerDto> sellersDtos = jsonString.DeserializeFromJson<List<ImportSellerDto>>();
            int[] gameIds = context.Boardgames
                .Select(bg=>bg.Id)
                .ToArray();
            foreach(var sellerDto in sellersDtos) 
            { 
                if(!IsValid(sellerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var seller = new Seller()
                {
                    Name = sellerDto.Name,
                    Address = sellerDto.Adderess,
                    Country = sellerDto.Country,
                    Website = sellerDto.Website
                };
                foreach(var boardgames in sellerDto.BoardgamesIds.Distinct())
                {
                    if (!gameIds.Contains(boardgames))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    } 
                    BoardgameSeller bgs = new()
                    {
                        Seller = seller,
                        BoardgameId = boardgames
                    };
                    seller.BoardgamesSellers.Add(bgs);
                }
                sellers.Add(seller);
                sb.AppendLine(string.Format(SuccessfullyImportedSeller, seller.Name,seller.BoardgamesSellers.Count()));
            }
            context.AddRange(sellers);
            context.SaveChanges();
            return sb.ToString();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
