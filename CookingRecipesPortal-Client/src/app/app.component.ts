import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  public data?: string;

  constructor(http: HttpClient) {
    http.get<string>('/api/accounts/test').subscribe(result => {
      this.data = result;
    }, error => console.error(error));
  }

  title = 'CookingRecipesPortal-Client';
}

