import { Component, inject } from '@angular/core';

import {
    LanguageService,
    Language
} from '../../../core/services/language.service';

@Component({
    selector: 'app-language-switcher',
    standalone: true,
    templateUrl: './language-switcher.html',
    styleUrl: './language-switcher.scss'
})
export class LanguageSwitcherComponent {

    private readonly languageService = inject(LanguageService);

    get language(): Language {
        return this.languageService.language;
    }

    setLanguage(language: Language): void {
        this.languageService.setLanguage(language);
    }
}