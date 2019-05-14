<template>
  <div id="app">
    <nav class="navbar is-transparent is-dark">
      <div class="navbar-brand">
        <a class="navbar-item">
          <img src="@/assets/Logo_Radicitus_White.png">
        </a>
      </div>
      <div class="navbar-start">
          <div class="navbar-item">
            <a class="has-text-white" href="/raffles">
              Raffles
            </a>
          </div>
      </div>
    </nav>
    <router-view></router-view>
  </div>
</template>

<style lang="scss">
// Import Bulma's core
@import "~bulma/sass/utilities/_all";

// Set your colors
$twitter: #4099FF;
$twitter-invert: findColorInvert($twitter);

// Setup $colors to use as bulma classes (e.g. 'is-twitter')
$colors: (
    "white": ($white, $black),
    "black": ($black, $white),
    "light": ($light, $light-invert),
    "dark": ($dark, $dark-invert),
    "info": ($info, $info-invert),
    "success": ($success, $success-invert),
    "warning": ($warning, $warning-invert),
    "danger": ($danger, $danger-invert),
    "twitter": ($twitter, $twitter-invert)
);

// Links
$link: $primary;
$link-invert: $primary-invert;
$link-focus-border: $primary;

// Import Bulma and Buefy styles
@import "~bulma";
@import "~buefy/src/scss/buefy";
</style>
