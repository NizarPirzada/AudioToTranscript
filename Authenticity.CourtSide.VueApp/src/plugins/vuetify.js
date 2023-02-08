import Vue from "vue";
import Vuetify from "vuetify/lib";

Vue.use(Vuetify);

export default new Vuetify({
    theme: {
        options: {
            customProperties: true,
        },
        dark: false,
        themes: {
            light: {
                primary: "#4CAF50",
                secondary: "#FFFFFF",
                accent: "#000000",
                error: "#D90C0C",
                info: "#7EBFFC",
                success: "#4A9A4A",
                warning: "#FFAC33",
                
            },
        },
    },
});
