import { inject, Injectable } from "@angular/core";
import { Translation, TranslocoLoader } from "@ngneat/transloco";
import { HttpClient } from "@angular/common/http";
import { Observable, tap } from "rxjs";

@Injectable({ providedIn: 'root' })
export class TranslocoHttpLoader implements TranslocoLoader {
  private http = inject(HttpClient);

  getTranslation(lang: string): Observable<Translation> {
    console.log('[TranslocoLoader] Loading translation for lang:', lang);
    return this.http.get<Translation>(`assets/i18n/${lang}.json`).pipe(
      tap({
        next: (data) => console.log('[TranslocoLoader] Success:', lang, data),
        error: (error) => console.error('[TranslocoLoader] Error loading:', lang, error)
      })
    );
  }
}
