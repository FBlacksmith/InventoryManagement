import { Action } from 'rxjs/internal/scheduler/Action';
import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { ZodSchema, ZodError } from 'zod';

export function zodValidator(schema: ZodSchema): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const result = schema.safeParse(control.value);

    if (result.success) {
      return null;
    }

    const errors: ValidationErrors = {};

    result.error.issues.forEach((issue) => {
      // Collect errors. Simple validation strategy:
      // If path is empty (root error) or length 0, it applies to the control itself if it's a FormControl,
      // or to the group if it's a FormGroup but typically this validator is attached to a FormGroup.
      
      const key = issue.path.join('.') || 'required';
      errors[key] = issue.message;
    });

    return errors;
  };
}
