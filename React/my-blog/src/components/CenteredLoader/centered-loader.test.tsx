import React from 'react';
import { cleanup, render, screen } from '@testing-library/react';
import { CenteredLoader } from './CenteredLoader';
import '@testing-library/jest-dom';

describe('CenteredLoader', () => {
    afterEach(() => cleanup())

    it('renders without crashing', () => {
        render(<CenteredLoader />);
        expect(screen.getByRole('progressbar')).toBeInTheDocument();
    });

    it('uses the default color when no color prop is passed', () => {
        // Arrange
        const { container } = render(<CenteredLoader/>);
        const loaderRoot = container.getElementsByClassName("MuiCircularProgress-root")[0]

        // Act
        const hasPrimaryColorStyle = loaderRoot.classList.contains("MuiCircularProgress-colorPrimary");

        // Assert
        expect(hasPrimaryColorStyle).toBeTruthy();
    });

    it('uses the color passed through the color prop', () => {
        // Arrange
        const { container } = render(<CenteredLoader color="error"/>);
        const loaderRoot = container.getElementsByClassName("MuiCircularProgress-root")[0]

        // Act
        const hasErrorColorClass = loaderRoot.classList.contains("MuiCircularProgress-colorError");

        // Assert
        expect(hasErrorColorClass).toBeTruthy();
    });

    it('sets the correct size when the sizePx prop is passed', () => {
        render(<CenteredLoader sizePx={50} />);
        const progressBar = screen.getByRole('progressbar');
        expect(progressBar).toHaveAttribute('style', expect.stringContaining('50px'));
    });

    it('applies the correct vertical margin', () => {
        const { container } = render(<CenteredLoader verticalMarginPx={100} />);
        const parentLoaderBlock = container.getElementsByClassName("container")[0]
        expect(parentLoaderBlock).toHaveStyle('margin: 100px auto');
    });

});
