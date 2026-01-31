import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-theme-test',
  standalone: true,
  imports: [CommonModule, MatButtonModule],
  template: `
    <div class="p-8 bg-slate-100 rounded-xl shadow-lg border border-slate-200">
      <h2 class="text-2xl font-bold text-primary mb-4">Tailwind & Material Integration</h2>
      <p class="text-slate-600 mb-6">
        This component validates that Tailwind utility classes and Angular Material components work together.
      </p>
      
      <div class="flex gap-4 items-center">
        <button mat-raised-button color="primary">
          Material Primary
        </button>
        
        <button mat-flat-button color="warn">
          Material Warn
        </button>
        
        <button class="px-4 py-2 bg-secondary text-white rounded-md hover:opacity-90 transition-opacity">
          Tailwind Secondary Button
        </button>
      </div>

      <div class="mt-6 p-4 bg-white rounded-lg border border-slate-100">
        <p class="text-sm text-tertiary">
          Text utilizing the <strong>tertiary</strong> color mapped from Material Design to Tailwind.
        </p>
      </div>
    </div>
  `,
  styles: []
})
export class ThemeTestComponent {}
