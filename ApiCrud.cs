using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FunctionApp1
{
    public class ApiCrud
    {
        private readonly AppDbContext _context;

        public ApiCrud(AppDbContext context)
        {
            _context = context;
        }

        //CRUD

        //GET metod som hämtar all data i database och listar upp dom i t.ex. postman

        [Function("ProductsGet")]
        public async Task<IActionResult> GetProducts(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")] HttpRequest req)
        {


            var products = await _context.Products.ToListAsync();
            return new OkObjectResult(products);
        }

        //POST metod för att skapa en ny data i databasen

        [Function("productPost")]
        public async Task<IActionResult> PostProduct([HttpTrigger(AuthorizationLevel.Function, "post", Route = "product")] HttpRequest req)
        {

            string requestData = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Products>(requestData);


            _context.Products.Add(data);
            await _context.SaveChangesAsync();
            return new OkObjectResult(data);
        }

        //GET/{id} hämta specifik data i databasen med hjälp av ett id och visa upp det i t.ex. postman

        [Function("GetProductById")]

        public async Task<IActionResult> GetItemById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "product/{id}")] HttpRequest req, int id)
        {
            var GetItemById = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (GetItemById == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(GetItemById);
        }

        //PUT metod för att göra en uppdatering på den specifika datan du hämtar med hjälp av ett id

        [Function("PutProduct")]

        public async Task<IActionResult> PutProduct(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "product/{id}")]
            HttpRequest req, int id)
        {
            var updateCartItem = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (updateCartItem == null)
            {
                return new NotFoundResult();
            }

            string requestData = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Products>(requestData);

            updateCartItem.Name = data.Name;
            updateCartItem.Price = data.Price;

            await _context.SaveChangesAsync();

            return new OkObjectResult(updateCartItem);
        }

        //DELETE raderar den specifika data du hämtar med hjälp av id

        [Function("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "product/{id}")]
            HttpRequest req, int id)
        {
            var deleteItem = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (deleteItem == null)
            {

                return new NotFoundResult();
            }

            _context.Products.Remove(deleteItem);
            await _context.SaveChangesAsync();
            return new OkObjectResult(deleteItem);
        }

    }
}
