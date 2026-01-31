import { FormControl, FormGroup } from '@angular/forms';
import { z } from 'zod';
import { zodValidator } from './zod-validator';

describe('ZodValidator', () => {
  const schema = z.object({
    name: z.string().min(3),
    age: z.number().min(18)
  });

  it('should return null when validation succeeds', () => {
    const validator = zodValidator(schema);
    const form = new FormGroup({
      name: new FormControl('John'),
      age: new FormControl(25)
    });

    const result = validator(form);
    expect(result).toBeNull();
  });

  it('should return errors when validation fails', () => {
    const validator = zodValidator(schema);
    const form = new FormGroup({
      name: new FormControl('Jo'), // Too short
      age: new FormControl(10)     // Too young
    });

    const result = validator(form);
    expect(result).not.toBeNull();
    // Assuming the validator currently maps paths like "name", "age" for flat object
    expect(result?.['name']).toBeDefined();
    expect(result?.['age']).toBeDefined();
  });
});
