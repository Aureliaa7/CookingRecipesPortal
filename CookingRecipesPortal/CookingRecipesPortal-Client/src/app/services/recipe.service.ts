import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PagedResponse } from '../interfaces/paged-response.interface';
import { RecipeDetails } from '../interfaces/recipe-details.interface';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {

  constructor(private httpClient: HttpClient) { }

  getRecipesByAuthorId(authorId: string) {
    console.log("authorId: ", authorId);
    return this.httpClient.get<PagedResponse<RecipeDetails>>(`/api/recipes/${authorId}`);
  }
}
