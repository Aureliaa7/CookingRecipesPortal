import { Component, OnInit } from '@angular/core';
import { PagedResponse } from '../../../interfaces/paged-response.interface';
import { RecipeDetails } from '../../../interfaces/recipe-details.interface';
import { AccountService } from '../../../services/account.service';
import { RecipeService } from '../../../services/recipe.service';

@Component({
  selector: 'app-user-recipes',
  templateUrl: './user-recipes.component.html',
  styleUrls: ['./user-recipes.component.css']
})
export class UserRecipesComponent implements OnInit {
  recipes: RecipeDetails[] = [];

  constructor(private recipeService: RecipeService, private accountService: AccountService) { }

  pageNumber: number = 1;
  totalPages: number = 1;

  ngOnInit(): void {
    const currentUserId = this.accountService.getCurrentUserId();
    this.recipeService.getRecipesByAuthorId(currentUserId).subscribe(
      pagedResponse => {
        console.log("pagedResponse: ", pagedResponse);
        this.pageNumber = pagedResponse.pageNumber;
        this.totalPages = pagedResponse.totalPages;
        this.recipes = pagedResponse.data;
      });
  }

  loadMoreRecipes(ev: any) {
    console.log("loadMore: ", ev);
    //TODO implement
  }

}
