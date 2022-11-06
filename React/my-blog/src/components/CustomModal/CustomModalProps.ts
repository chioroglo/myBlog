import React from "react";

export interface CustomModalProps {
    modalOpen: boolean,
    setModalOpen: React.Dispatch<React.SetStateAction<boolean>>,
    children?: React.ReactNode,
    title?: string
}