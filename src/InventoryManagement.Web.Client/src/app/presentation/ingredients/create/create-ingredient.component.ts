import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { Mediator } from 'mediatr-ts';
import { MEASUREMENT_UNITS, MEASUREMENT_UNIT_OPTIONS } from '@core/models/measurement-unit.model';
import { zodValidator } from '@core/validators/zod-validator';
import { createIngredientSchema, CreateIngredientSchema } from '@app/ingredients/create/create-ingredient.schema';
import { CreateIngredientRequest } from '@app/ingredients/create/create-ingredient.handler';

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
    MatCardModule
  ],
  template: `
    <div class="p-6 max-w-lg mx-auto">
      <mat-card class="p-4">
        <mat-card-header>
          <mat-card-title class="text-xl font-bold mb-4">Create New Ingredient</mat-card-title>
        </mat-card-header>
        
        <mat-card-content>
          <form [formGroup]="form" (ngSubmit)="onSubmit()" class="flex flex-col gap-4">
            
            <!-- Name Field -->
            <mat-form-field appearance="outline" class="w-full">
              <mat-label>Ingredient Name</mat-label>
              <input matInput formControlName="name" placeholder="Ex: Flour">
              @if (form.controls['name'].hasError('min')) {
                <mat-error>{{ form.controls['name'].getError('min') }}</mat-error>
              }
               @if (form.controls['name'].hasError('required')) {
                <mat-error>Name is required</mat-error>
              }
            </mat-form-field>

            <!-- Measurement Unit Field -->
            <mat-form-field appearance="outline" class="w-full">
              <mat-label>Measurement Unit</mat-label>
              <mat-select formControlName="measurementUnit">
                @for (unit of unitOptions; track unit) {
                  <mat-option [value]="unit">{{ unit }}</mat-option>
                }
              </mat-select>
              @if (form.controls['measurementUnit'].hasError('invalid_enum_value')) {
                 <mat-error>Invalid unit selected</mat-error>
              }
              @if (form.controls['measurementUnit'].hasError('required')) {
                <mat-error>Unit is required</mat-error>
              }
            </mat-form-field>

            <!-- Submit Button -->
             <div class="flex justify-end mt-4">
              <button mat-raised-button color="primary" type="submit" [disabled]="form.invalid || isSubmitting()">
                {{ isSubmitting() ? 'Saving...' : 'Create Ingredient' }}
              </button>
             </div>
             
             @if (errorMessage()) {
                <div class="text-red-500 mt-2">{{ errorMessage() }}</div>
             }
          </form>
        </mat-card-content>
      </mat-card>
    </div>
  `
})
export class CreateIngredientComponent {
  private fb = inject(FormBuilder);
  private mediator = inject(Mediator);
  
  unitOptions = MEASUREMENT_UNIT_OPTIONS;
  isSubmitting = signal(false);
  errorMessage = signal<string | null>(null);

  form = this.fb.group({
    name: ['', [Validators.required]], // Zod validator set on group or control? 
    // Usually mix standard require with Zod. 
    // Ideally use zodValidator for the whole group or per control.
    // Let's attach zodValidator to the group for schema validation or individual controls.
    // For specific fields validation, it's better to attach to fields.
    measurementUnit: ['', [Validators.required]]
  });

  constructor() {
    // Apply validator to the form group for overall schema check if needed,
    // OR apply strictly to controls. The custom validator I wrote handles paths.
    // Let's refactor to use strict types and maybe helper.
    // For now, attaching the Zod validator to the form group is the cleanest for full schema.
    this.form.addValidators(zodValidator(createIngredientSchema));
  }

  async onSubmit() {
    if (this.form.invalid) return;

    this.isSubmitting.set(true);
    this.errorMessage.set(null);

    const formData = this.form.value as CreateIngredientSchema;

    try {
      const request = new CreateIngredientRequest(formData);
      await this.mediator.send(request);
      
      this.form.reset();
      // Optionally show success message (snackbar)
      alert('Ingredient created successfully!'); 
    } catch (error: any) {
      console.error(error);
      this.errorMessage.set(error.message || 'Failed to create ingredient');
    } finally {
      this.isSubmitting.set(false);
    }
  }
}
