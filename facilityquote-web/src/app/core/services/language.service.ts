import { Injectable, inject } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

export type Language = 'de' | 'en';

@Injectable({
    providedIn: 'root'
})
export class LanguageService {

    private readonly translate = inject(TranslateService);

    private currentLanguage: Language;

    constructor() {
        const savedLanguage = localStorage.getItem('language');

        this.currentLanguage =
            savedLanguage === 'en' ? 'en' : 'de';

        this.translate.use(this.currentLanguage);
    }

    get language(): Language {
        return this.currentLanguage;
    }

    setLanguage(language: Language): void {
        this.currentLanguage = language;

        localStorage.setItem('language', language);

        this.translate.use(language);
    }
}