using AutoMapper;
using CookingRecipesPortal_DAL.DomainModels;
using CookingRecipesPortal_DAL.DTOs;
using CookingRecipesPortal_DAL.Interfaces.Services;
using CookingRecipesPortal_DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookingRecipesPortal_API.Controllers
{
    public class RecipesController : CookingRecipesPortalController
    {
        private readonly ICookingRecipeService cookingRecipeService;
        private readonly IMapper mapper;

        public RecipesController(ICookingRecipeService cookingRecipeService, IMapper mapper)
        {
            this.cookingRecipeService = cookingRecipeService;
            this.mapper = mapper;
        }

        [HttpPost("{userId}")]
        [Authorize]
        public async Task<IActionResult> CreateRecipe([FromRoute] Guid userId, RecipeDto recipe)
        {
            var addedRecipe = await cookingRecipeService.AddAsync(userId, mapper.Map<Recipe>(recipe), recipe.Base64Images);
            return Ok(mapper.Map<RecipeDto>(addedRecipe)); 
        }

        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetRecipesByAuthor([FromRoute] Guid userId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var recipes = await cookingRecipeService.GetByAuthorAsync(userId, new PaginationFilter(pageNumber, pageSize));
            return Ok(recipes);
        }

        [HttpPut("{userId}/edit")]
        [Authorize]
        public async Task<IActionResult> EditRecipe([FromRoute] Guid userId, [FromBody] UpdateRecipeModel updatedRecipe)
        {
            var recipe = await cookingRecipeService.UpdateAsync(userId, updatedRecipe);
            return Ok(recipe);
        }

        [HttpDelete("{userId}/delete/{recipeId}")]
        [Authorize]
        public async Task<IActionResult> DeleteRecipe([FromRoute] Guid userId, [FromRoute] Guid recipeId)
        {
            await cookingRecipeService.DeleteAsync(userId, recipeId);
            return NoContent();
        }

        [HttpPost("{userId}/save/{recipeId}")]
        [Authorize]
        public async Task<IActionResult> MarkAsSavedRecipe([FromRoute] Guid userId, [FromRoute] Guid recipeId)
        {
            await cookingRecipeService.SaveRecipeAsync(userId, recipeId);
            return Ok();
        }

        [HttpPost("{userId}/like/{recipeId}")]
        [Authorize]
        public async Task<IActionResult> MarkAsLikedRecipe([FromRoute] Guid userId, [FromRoute] Guid recipeId)
        {
            await cookingRecipeService.LikeRecipeAsync(userId, recipeId);
            return Ok();
        }


        [HttpDelete("{userId}/remove-like/{recipeId}")]
        [Authorize]
        public async Task<IActionResult> RemoveFromLikedRecipes([FromRoute] Guid userId, [FromRoute] Guid recipeId)
        {
            await cookingRecipeService.RemoveFromLikedRecipesAsync(userId, recipeId);
            return Ok();
        }

        [HttpDelete("{userId}/remove-from-saved-recipes/{recipeId}")]
        [Authorize]
        public async Task<IActionResult> RemoveFromSavedRecipes([FromRoute] Guid userId, [FromRoute] Guid recipeId)
        {
            await cookingRecipeService.RemoveFromSavedRecipesAsync(userId, recipeId);
            return Ok();
        }

        [HttpGet("{userId}/saved-recipes")]
        [Authorize]
        public async Task<IActionResult> ViewSavedRecipes([FromRoute] Guid userId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var savedRecipes = await cookingRecipeService.GetSavedRecipesAsync(userId, new PaginationFilter(pageNumber, pageSize));
            return Ok(savedRecipes);
        }
    }
}
