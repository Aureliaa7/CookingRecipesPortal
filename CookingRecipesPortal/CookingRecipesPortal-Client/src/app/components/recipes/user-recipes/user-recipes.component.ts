import { Component, OnInit } from '@angular/core';
import { RecipeDetails } from '../../../interfaces/recipe-details.interface';
import { RecipeService } from '../../../services/recipe.service';

@Component({
  selector: 'app-user-recipes',
  templateUrl: './user-recipes.component.html',
  styleUrls: ['./user-recipes.component.css']
})
export class UserRecipesComponent implements OnInit {
  recipes: RecipeDetails[] = [];

  constructor(private recipeService: RecipeService) { }

  ngOnInit(): void {
    this.recipeService.getRecipesByAuthorId().subscribe(
      pagedResponse => {
        console.log("pagedResponse: ", pagedResponse);
        this.recipes = pagedResponse.data;
      });
  }

}
