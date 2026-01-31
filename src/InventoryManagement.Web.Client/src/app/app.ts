import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ThemeTestComponent } from './theme-test/theme-test.component';
import { CreateIngredientComponent } from './presentation/ingredients/create/create-ingredient.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ThemeTestComponent, CreateIngredientComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('InventoryManagement.Web.Client');
}
