import { createTheme } from "@mui/material/styles";

export const applicationTheme = createTheme({
    palette: {
        mode: "light",
        primary: { main: "#365486", dark: "#243b63", light: "#6d8fbd" },
        secondary: { main: "#3aa6a0", dark: "#267a76", light: "#87d2cd" },
        background: { default: "#f4f7fb", paper: "#ffffff" },
        text: { primary: "#182337", secondary: "#657187" },
        divider: "rgba(24, 35, 55, 0.10)"
    },
    shape: { borderRadius: 14 },
    typography: {
        fontFamily: '"Onest", "Segoe UI", sans-serif',
        h1: { fontWeight: 750, letterSpacing: "-0.04em" },
        h2: { fontWeight: 700, letterSpacing: "-0.035em" },
        h3: { fontWeight: 700, letterSpacing: "-0.025em" },
        h4: { fontWeight: 700, letterSpacing: "-0.02em" },
        h5: { fontWeight: 650, letterSpacing: "-0.015em" },
        h6: { fontWeight: 650 },
        button: { fontWeight: 650, letterSpacing: "0.01em", textTransform: "none" }
    },
    cssVariables: true,
    components: {
        MuiCssBaseline: {
            styleOverrides: {
                body: { minWidth: 320, backgroundColor: "#f4f7fb" },
                "::selection": { backgroundColor: "#c9e4f4", color: "#182337" }
            }
        },
        MuiAppBar: {
            styleOverrides: {
                root: {
                    backgroundImage: "none",
                    backgroundColor: "rgba(36, 59, 99, 0.94)",
                    backdropFilter: "blur(16px)",
                    boxShadow: "0 8px 30px rgba(24, 35, 55, 0.16)"
                }
            }
        },
        MuiToolbar: { styleOverrides: { root: { minHeight: 68, width: "100%", maxWidth: 1180, margin: "0 auto" } } },
        MuiPaper: { styleOverrides: { root: { backgroundImage: "none" } } },
        MuiCard: {
            styleOverrides: {
                root: { border: "1px solid rgba(24, 35, 55, 0.08)", boxShadow: "0 12px 36px rgba(24, 35, 55, 0.09)" }
            }
        },
        MuiButton: {
            defaultProps: { disableElevation: true },
            styleOverrides: { root: { minHeight: 42, borderRadius: 11, paddingInline: 18 } }
        },
        MuiIconButton: { styleOverrides: { root: { borderRadius: 10 } } },
        MuiTextField: { defaultProps: { size: "small" } },
        MuiOutlinedInput: { styleOverrides: { root: { borderRadius: 11, backgroundColor: "rgba(255,255,255,.72)" } } },
        MuiDialog: {
            styleOverrides: { paper: { borderRadius: 20, boxShadow: "0 24px 80px rgba(17, 28, 46, .24)" } }
        },
        MuiDialogTitle: { styleOverrides: { root: { fontSize: "1.3rem", fontWeight: 700, padding: "24px 24px 12px" } } },
        MuiTabs: { styleOverrides: { root: { borderBottom: "1px solid rgba(24,35,55,.1)" } } },
        MuiTab: { styleOverrides: { root: { minHeight: 52, textTransform: "none", fontWeight: 650 } } }
    }
})
