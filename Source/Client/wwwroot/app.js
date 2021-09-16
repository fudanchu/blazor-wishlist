window.app = {
    loadScript: (sourceSrc) => {
        var scriptTag = document.createElement('script');
        scriptTag.src = sourceSrc;
        document.getElementsByTagName('head')[0].appendChild(scriptTag);
    },
    speak: (message, defaultVoice, voiceSpeed, lang) => {
        if (message == "") {
            //HACK: get last message in chat box
            var messageCount = document.getElementsByClassName("chat-box")[0].getElementsByTagName("li").length;
            message = document.getElementsByClassName("chat-box")[0].getElementsByTagName("li")[messageCount - 1].innerText;
        }
        const utterance = new SpeechSynthesisUtterance(message);
        const voices = window.speechSynthesis.getVoices();
        try {
            utterance.voice =
                !!defaultVoice && defaultVoice !== 'Auto'
                    ? voices.find(v => v.name === defaultVoice)
                    : voices.find(v => !!lang && v.lang.startsWith(lang) || v.name === 'Google US English') || voices[0];
        } catch { }
        utterance.volume = 1;
        utterance.rate = voiceSpeed || 1;

        window.speechSynthesis.speak(utterance);
    },
    toggleSettingsModal: (newStyle) => {
        const element = document.querySelector('#chatSettingsModal');
        if (newStyle != null) {
            element.style.display = newStyle;
        } else {
            if (element.style.display == "block") {
                element.style.display = "none";
            } else {
                element.style.display = "block";
            }
        }
    },
    updateScroll: () => {
        //HACK: adding delay because otherwise scroll may occur before new message renders
        setTimeout(() => {
            const element = document.querySelector('.chat-box');
            if (element) {
                element.scrollTop = element.scrollHeight;
            }
        }, 100);
    },
    getClientVoices: dotnetObj => {
        let voices = speechSynthesis.getVoices();
        if (!voices || !voices.length) {
            speechSynthesis.onvoiceschanged = () => {
                voices = speechSynthesis.getVoices();
                dotnetObj.invokeMethodAsync(
                    "UpdateClientVoices",
                    JSON.stringify(voices.map(
                        voice => ({ Name: voice.name, Lang: voice.lang, Default: voice.default }))));
            }
        }

        return JSON.stringify(voices.map(v => ({ Name: v.name, Lang: v.lang, Default: v.default })));
    }
};

// Prevent bots from speaking when user closes tab or window.
window.addEventListener('beforeunload', _ => {
    if (window.speechSynthesis && window.speechSynthesis.pending === true) {
        window.speechSynthesis.cancel();
    }
});
document.addEventListener('click', function (e) {
    if (e.target.className === 'modal') {
        app.toggleSettingsModal('none');
    }
}, false);