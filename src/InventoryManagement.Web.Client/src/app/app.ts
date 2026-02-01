import { Component, inject, signal, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ThemeTestComponent } from './theme-test/theme-test.component';
import { CreateIngredientComponent } from './presentation/ingredients/create/create-ingredient.component';
import { TranslocoService } from '@ngneat/transloco';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ThemeTestComponent, CreateIngredientComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private translocoService = inject(TranslocoService);
  protected readonly title = signal('InventoryManagement.Web.Client');

  ngOnInit() {
    this.translocoService.setActiveLang('pt-BR');
  }
}
