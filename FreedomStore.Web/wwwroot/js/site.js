$(document).ready(function () {
    // Elementos principais
    const $menuToggle = $('.menu-toggle');
    const $sidebar = $('.sidebar');
    const $mainContent = $('.main-content');
    const $sidebarHeader = $('.sidebar-header');

    // Toggle do menu em dispositivos móveis
    $menuToggle.on('click', function () {
        $sidebar.addClass('active');
    });


    // Collapse/Expand ao clicar na logo
    $sidebarHeader.on('click', function (e) {
        // Evita que o evento seja acionado ao clicar no botão de fechar
        if ($(e.target).hasClass('close-btn') || $(e.target).parent().hasClass('close-btn')) {
            return;
        }

        // Verifica se a largura da tela é maior que 768px
        if ($(window).width() > 768) {
            $sidebar.toggleClass('collapsed');
            $mainContent.toggleClass('collapsed');
        } else {
            // Em telas pequenas, fecha o menu ao clicar no header
            $sidebar.removeClass('active');
        }
    });

    // Listener para redimensionamento da janela
    $(window).on('resize', function () {
        // Se a largura for menor ou igual a 768px, força o menu a não estar collapsed
        if ($(window).width() <= 768) {
            $sidebar.removeClass('collapsed');
            $mainContent.removeClass('collapsed');
        }
    });

    // Submenus
    const $menuItemsWithSubmenu = $('.menu-item.with-submenu');

    $menuItemsWithSubmenu.each(function () {
        const $submenuTrigger = $(this).find('.chevron');
        const $item = $(this);

        $item.on('click', function (e) {
            // Evita fechar o submenu quando clica no ícone de seta
            if (e.target === $submenuTrigger[0]) return;

            const $submenu = $(this).next('.submenu');
            const isActive = $submenu.hasClass('active');

            // Fecha todos os submenus primeiro
            $('.submenu').removeClass('active');
            $('.chevron').removeClass('fa-chevron-up').addClass('fa-chevron-down');

            // Abre o submenu atual se não estava ativo
            if (!isActive) {
                $submenu.addClass('active');
                $(this).find('.chevron').removeClass('fa-chevron-down').addClass('fa-chevron-up');
            }
        });

        // Adiciona evento separado para o ícone de seta
        $submenuTrigger.on('click', function (e) {
            e.stopPropagation();
            const $submenu = $(this).parent().next('.submenu');
            const isActive = $submenu.hasClass('active');

            // Fecha todos os outros submenus
            $('.submenu').not($submenu).removeClass('active');
            $('.chevron').not(this).removeClass('fa-chevron-up').addClass('fa-chevron-down');

            // Alterna o estado do submenu atual
            $submenu.toggleClass('active');

            if ($submenu.hasClass('active')) {
                $(this).removeClass('fa-chevron-down').addClass('fa-chevron-up');
            } else {
                $(this).removeClass('fa-chevron-up').addClass('fa-chevron-down');
            }
        });
    });

    // Ativar item do menu
    const $allMenuItems = $('.menu-item:not(.with-submenu), .submenu-item');

    $allMenuItems.on('click', function () {
        // Remove active class from all items
        $('.menu-item').removeClass('active');
        $('.submenu-item').removeClass('active');

        // Add active class to clicked item
        $(this).addClass('active');

        // If it's a submenu item, also activate its parent
        if ($(this).hasClass('submenu-item')) {
            $(this).closest('.submenu').prev('.menu-item').addClass('active');
        } else {
            // If it's a main menu item, close all submenus
            $('.submenu').removeClass('active');
            $('.chevron').removeClass('fa-chevron-up').addClass('fa-chevron-down');
        }
    });

    // Botão de logout
    const $logoutBtn = $('.logout-btn');

    $logoutBtn.on('click', function () {
        alert('Função de logout acionada!');
        // Aqui você pode adicionar a lógica real de logout
        // window.location.href = '/logout';
    });
});

