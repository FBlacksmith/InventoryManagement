import { TestBed } from '@angular/core/testing';
import { AngularMediatorResolver } from './angular-mediator-resolver';
import { Injectable } from '@angular/core';

@Injectable()
class SimpleHandler {
  handle() { return 'handled'; }
}

@Injectable()
class DependencyService {
  getValue() { return 'dependency-value'; }
}

@Injectable()
class DependentHandler {
  constructor(private service: DependencyService) {}
  handle() { return this.service.getValue(); }
}

describe('AngularMediatorResolver', () => {
  let resolver: AngularMediatorResolver;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        AngularMediatorResolver,
        SimpleHandler,
        DependentHandler,
        DependencyService
      ]
    });
    resolver = TestBed.inject(AngularMediatorResolver);
  });

  it('should resolve a simple handler', () => {
    const handler = resolver.resolve<SimpleHandler>(SimpleHandler);
    expect(handler).toBeTruthy();
    expect(handler.handle()).toBe('handled');
  });

  it('should resolve a handler with dependencies', () => {
    const handler = resolver.resolve<DependentHandler>(DependentHandler);
    expect(handler).toBeTruthy();
    expect(handler.handle()).toBe('dependency-value');
  });

  it('should throw error when resolving unregistered handler', () => {
    class UnregisteredHandler {}
    
    expect(() => {
      resolver.resolve(UnregisteredHandler);
    }).toThrow();
  });
});
