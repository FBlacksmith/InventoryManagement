import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MEASUREMENT_UNIT_OPTIONS } from '@core/models/measurement-unit.model';
import { zodValidator } from '@core/validators/zod-validator';
import { createIngredientSchema, CreateIngredientSchema } from '@app/ingredients/create/create-ingredient.schema';
import { CreateIngredientRequest } from '@app/ingredients/create/create-ingredient.handler';
import { Mediator } from 'mediatr-ts';
import { NotificationService } from '@core/services/notification.service';
import { TranslocoPipe, TranslocoService } from '@ngneat/transloco';

@Component({
  selector: 'app-create-ingredient',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatCardModule,
    TranslocoPipe
  ],
  template: `
    <div class="p-6 max-w-lg mx-auto">
      <mat-card class="p-4">
        <mat-card-header>
          <mat-card-title class="text-xl font-bold mb-4">{{ 'ingredients.title' | transloco }}</mat-card-title>
        </mat-card-header>
        
        <mat-card-content>
          <form [formGroup]="form" (ngSubmit)="onSubmit()" class="flex flex-col gap-4">
            
            <!-- Name Field -->
            <mat-form-field appearance="outline" class="w-full">
              <mat-label>{{ 'ingredients.name' | transloco }}</mat-label>
              <input matInput formControlName="name" placeholder="Ex: Flour">
              @if (form.controls['name'].hasError('required')) {
                 <mat-error>{{ 'validation.required' | transloco }}</mat-error>
              }
              @if (form.errors?.['name']) {
                <mat-error>{{ form.errors?.['name'] | transloco: { value: 3 } }}</mat-error>
              }
            </mat-form-field>

            <!-- Measurement Unit Field -->
            <mat-form-field appearance="outline" class="w-full">
              <mat-label>{{ 'ingredients.unit' | transloco }}</mat-label>
              <mat-select formControlName="measurementUnit">
                @for (unit of unitOptions; track unit) {
                  <mat-option [value]="unit">{{ 'measurement_units.' + unit | transloco }}</mat-option>
                }
              </mat-select>
              @if (form.errors?.['measurementUnit']) {
                 <mat-error>{{ form.errors?.['measurementUnit'] | transloco }}</mat-error>
              }
              @if (form.controls['measurementUnit'].hasError('required')) {
                <mat-error>{{ 'validation.required' | transloco }}</mat-error>
              }
            </mat-form-field>

            <!-- Submit Button -->
             <div class="flex justify-end mt-4">
              <button mat-raised-button color="primary" type="submit" [disabled]="form.invalid || isLoading()">
                {{ isLoading() ? ('ingredients.save' | transloco) + '...' : ('ingredients.save' | transloco) }}
              </button>
             </div>
          </form>
        </mat-card-content>
      </mat-card>
    </div>
  `
})
export class CreateIngredientComponent {
  private fb = inject(FormBuilder);
  private mediator = inject(Mediator);
  private notification = inject(NotificationService);
  private transloco = inject(TranslocoService);

  readonly isLoading = signal(false);

  unitOptions = MEASUREMENT_UNIT_OPTIONS;

  form = this.fb.group({
    name: ['', [Validators.required]],
    measurementUnit: ['', [Validators.required]]
  });

  constructor() {
    this.form.addValidators(zodValidator(createIngredientSchema));

  }

  async onSubmit() {
    if (this.form.invalid) return;

    this.isLoading.set(true);

    try {
      await this.mediator.send(
        new CreateIngredientRequest(this.form.value as CreateIngredientSchema)
      );

      this.notification.success('Ingredient created successfully!');
      this.form.reset();
    } finally {
      this.isLoading.set(false);
    }
  }
}

