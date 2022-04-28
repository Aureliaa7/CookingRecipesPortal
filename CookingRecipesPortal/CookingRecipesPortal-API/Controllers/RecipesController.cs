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
            var addedRecipe = await cookingRecipeService.AddAsync(userId, mapper.Map<Recipe>(recipe));
            return Ok(mapper.Map<RecipeDto>(addedRecipe)); 
        }

        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetAll([FromRoute] Guid userId)
        {
            var recipes = await cookingRecipeService.GetByAuthorAsync(userId);
            return Ok(mapper.Map<IList<RecipeDto>>(recipes));
        }

        [HttpPut("{userId}/edit")]
        [Authorize]
        public async Task<IActionResult> EditRecipe([FromRoute] Guid userId, [FromBody] UpdateRecipeModel updatedRecipe)
        {
            var recipe = await cookingRecipeService.UpdateAsync(userId, updatedRecipe);
            return Ok(mapper.Map<RecipeDto>(recipe));
        }

        [HttpDelete("{userId}/delete/{recipeId}")]
        [Authorize]
        public async Task<IActionResult> DeleteRecipe([FromRoute] Guid userId, [FromRoute] Guid recipeId)
        {
            await cookingRecipeService.DeleteAsync(userId, recipeId);
            return NoContent();
        }
    }
}
